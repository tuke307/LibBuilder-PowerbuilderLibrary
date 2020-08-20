// project=LibBuilder.WPFCore, file=ContentViewModel.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore.ViewModels
{
    using Data.Models;
    using LibBuilder.WPFCore.Business;
    using LibBuilder.WPFCore.Views;
    using MaterialDesignColors.Recommended;
    using MaterialDesignThemes.Wpf;
    using Microsoft.Win32;
    using MvvmCross.Commands;
    using MvvmCross.Core;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// ContentViewModel.
    /// </summary>
    /// <seealso cref="LibBuilder.Core.ViewModels.ContentViewModel" />
    public class ContentViewModel : LibBuilder.Core.ViewModels.ContentViewModel
    {
        protected MainWindowViewModel mainWindowViewModel;
        protected Options parameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentViewModel" /> class.
        /// </summary>
        /// <param name="mainWindowViewModel">The main window view model.</param>
        /// <param name="parameter">The parameter.</param>
        public ContentViewModel(MainWindowViewModel mainWindowViewModel = null, Options parameter = null)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.parameter = parameter;

            //Laden
            WorkspaceSelectedCommand = new MvxAsyncCommand(async () => await LoadWorkspace());
            TargetSelectedCommand = new MvxAsyncCommand(async () => await LoadTarget());
            LibrarySelectedCommand = new MvxAsyncCommand(async () => await LoadLibrary());

            //UI bezogen
            OpenWorkspaceCommand = new MvxAsyncCommand(OpenWorkspace);

            RunProcedurCommand = new MvxAsyncCommand(RunProcedurAsync);

            if (parameter != null)
            {
                Task.Run(ParameterStartAsync);
            }
            //letzten modifizierten Workspace laden, mit zuletzt ausgewähltem Target
            //if (Workspaces != null && Workspaces.Count > 0)
            //    Workspace = Workspaces.OrderByDescending(w => w.UpdatedDate).FirstOrDefault();
        }

        #region Methods

        /// <summary>
        /// Lädt Library aus Datenbank Vergleich mit aktueller Powerbuilder Library
        /// Gegebenfalls löschen oder hinzufügen neuer Objects(Entrys)
        /// </summary>
        protected override async Task LoadLibrary()
        {
            ContentLoadingAnimation = true;
            await RaisePropertyChanged(() => ContentLoadingAnimation);

            await base.LoadLibrary();

            ContentLoadingAnimation = false;
            await RaisePropertyChanged(() => ContentLoadingAnimation);
        }

        /// <summary>
        /// Lädt Target aus Datenbank Vergleich mit aktuellem Powerbuilder Target
        /// gegenbenfalls werden neue Librarys hinzugefügt oder alte gelöscht
        /// </summary>
        protected override async Task LoadTarget()
        {
            ContentLoadingAnimation = true;
            await RaisePropertyChanged(() => ContentLoadingAnimation);

            await base.LoadTarget();

            ContentLoadingAnimation = false;
            await RaisePropertyChanged(() => ContentLoadingAnimation);
        }

        /// <summary>
        /// Lädt Workspace aus Datenbank Vergleich mit aktuellem Powerbuilder Workspace.
        /// Gegenbenfalls werden neue Targets hinzugefügt oder alte gelöscht
        /// </summary>
        protected override async Task LoadWorkspace()
        {
            if (!await CheckWorkspaceAsync())
            {
                return;
            }

            ContentLoadingAnimation = true;
            await RaisePropertyChanged(() => ContentLoadingAnimation);

            await base.LoadWorkspace();

            ContentLoadingAnimation = false;
            await RaisePropertyChanged(() => ContentLoadingAnimation);
        }

        /// <summary>
        /// Opens the workspace.
        /// </summary>
        protected async Task OpenWorkspace()
        {
            ContentLoadingAnimation = true;
            await RaisePropertyChanged(() => ContentLoadingAnimation);

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Powerbuilder Workspace (*.pbw)|*.pbw";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog().HasValue)
            {
                base.OpenWorkspace(openFileDialog.FileName);
            }

            ContentLoadingAnimation = false;
            await RaisePropertyChanged(() => ContentLoadingAnimation);
        }

        /// <summary>
        /// Parameters the start asynchronous.
        /// </summary>
        protected async Task ParameterStartAsync()
        {
            Console.WriteLine();
            Console.WriteLine("---------Vorbereitung---------");

            // Workspace
            if (parameter.Workspace != null)
            {
                // pfad
                if (Path.IsPathFullyQualified(parameter.Workspace) && !Path.IsPathRooted(parameter.Workspace))
                {
                    string filePath = Path.GetFullPath(parameter.Workspace);

                    if (File.Exists(filePath))
                    {
                        try
                        {
                            Workspace = Workspaces.Single(w => w.FilePath.ToLower() == filePath.ToLower());
                        }
                        catch
                        {
                            // in Datenbank hinzufügen
                            Console.WriteLine("Workspace-Pfad konnte in Datenbank nicht gefunden werden, daher wird dieser hinzugefügt");
                            base.OpenWorkspace(filePath);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Der Workspace-Pfad " + filePath + " ist ungültig");
                        return;
                    }
                }
                // Name
                else
                {
                    string fileName = Path.GetFileName(parameter.Workspace);

                    try
                    {
                        Workspace = Workspaces.Single(w => w.File.ToLower() == fileName.ToLower());
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Wokspace konnte in Datenbank nicht gefunden werden; " + exc.Message);
                        return;
                    }
                }

                await LoadWorkspace();
                Console.WriteLine("Workspace " + Workspace.FilePath + " erfolgreich eingelesen");
            }
            else
            {
                // ohne Workspace geht nicht
                Console.WriteLine("Bitte Workspace angeben");
                return;
            }

            // Version
            if (parameter.Version != null)
            {
                Workspace.PBVersion = parameter.Version;

                await base.SaveWorkspace();
                await LoadWorkspace();

                Console.WriteLine("Version erfolgreich gesetzt");
            }
            else
            {
                if (!Workspace.PBVersion.HasValue)
                {
                    Console.WriteLine("Bitte Powerbuilder-Version für Workspace angeben");
                    return;
                }

                // Version, die in DB gespeichert ist
            }

            // Target
            if (parameter.Target != null)
            {
                // pfad
                if (Path.IsPathFullyQualified(parameter.Target) && !Path.IsPathRooted(parameter.Target))
                {
                    string filePath = Path.GetFullPath(parameter.Target);

                    if (File.Exists(filePath))
                    {
                        try
                        {
                            Target = Targets.Single(t => t.FilePath.ToLower() == filePath.ToLower());
                        }
                        catch
                        {
                            // in Datenbank hinzufügen
                            Console.WriteLine("Target-Pfad konnte in dem Worspace nicht gefunden werden nicht gefunden werden");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Der Target-Pfad " + filePath + " ist ungültig");
                        return;
                    }
                }
                // name
                else
                {
                    string fileName = Path.GetFileName(parameter.Target);

                    Target = Targets.Single(t => t.File.ToLower() == fileName.ToLower());
                }

                await LoadTarget();
                Console.WriteLine("Target " + Target.FilePath + " erfolgreich eingelesen");
            }
            else
            {
                // zuletzt ausgewähltes Target laden
                Target = Targets.OrderByDescending(t => t.UpdatedDate).FirstOrDefault();
                await LoadTarget();
                Console.WriteLine("zuletzt ausgewähltes Target " + Target.FilePath + " erfolgreich eingelesen");
            }

            // Application Rebuild
            if (parameter.RebuildType.HasValue)
            {
                Target.ApplicationRebuild = parameter.RebuildType.Value;
                await base.SaveTarget();

                // Library Build und Library-Object Regenerate überspringen
                goto Run;
            }
            else
            {
                // zurücksetzen
                Target.ApplicationRebuild = null;
                await base.SaveTarget();
            }

            // ausgewählte Librarys - Build/Regenerate
            if (!parameter.Librarys.IsNullOrEmpty() && (parameter.Build.HasValue || parameter.Regenerate.HasValue))
            {
                // ausgewählte Librarys

                Console.WriteLine("Librarys " + String.Join(", ", parameter.Librarys.ToArray()) + " selektiert");

                // build deselktieren
                base.DeselectAllLibrarys();

                // Regenerate deselektieren
                foreach (var lib in Librarys)
                {
                    Library = Librarys.Single(x => x == lib);

                    // Library Objects laden
                    await LoadLibrary();

                    base.DeselectAllEntrys();
                }

                // ausgewählte Librarys
                foreach (var lib in parameter.Librarys)
                {
                    // pfad
                    if (Path.IsPathFullyQualified(lib) && !Path.IsPathRooted(lib))
                    {
                        string filePath = Path.GetFullPath(lib);

                        if (File.Exists(filePath))
                        {
                            try
                            {
                                Library = Librarys.Single(l => l.FilePath.ToLower() == filePath.ToLower());
                            }
                            catch
                            {
                                Console.WriteLine("Library-Pfad; " + filePath + ", wurde nicht gefunden");
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Der Library-Pfad " + filePath + " ist ungültig");
                            return;
                        }
                    }
                    // Name
                    else
                    {
                        string fileName = Path.GetFileName(lib);

                        try
                        {
                            Library = Librarys.Single(l => l.File.ToLower() == fileName.ToLower());
                        }
                        catch
                        {
                            Console.WriteLine("Library-Name; " + fileName + ", wurde nicht gefunden");
                            return;
                        }
                    }

                    // Library Objects laden
                    await LoadLibrary();

                    // Build
                    if (parameter.Build.HasValue)
                    {
                        if (!parameter.Build.Value)
                        {
                            Library.Build = false;
                        }
                        else
                        {
                            Library.Build = true;
                        }

                        await base.SaveLibrary();
                    }

                    // Regenerate
                    if (parameter.Regenerate.HasValue)
                    {
                        if (!parameter.Regenerate.Value)
                        {
                            base.DeselectAllEntrys();
                        }
                        else
                        {
                            base.SelectAllEntrys();
                        }
                    }
                }
            }
            // alle Librarys - Build/Regenerate
            else
            {
                // Build
                if (parameter.Build.HasValue)
                {
                    if (!parameter.Build.Value)
                    {
                        base.DeselectAllLibrarys();
                    }
                    else
                    {
                        base.SelectAllLibrarys();
                    }
                }

                // Regenerate
                if (parameter.Regenerate.HasValue)
                {
                    foreach (var lib in Librarys)
                    {
                        Library = Librarys.Single(x => x == lib);

                        // Library Objects laden
                        await LoadLibrary();

                        if (!parameter.Regenerate.Value)
                        {
                            base.DeselectAllEntrys();
                        }
                        else
                        {
                            base.SelectAllEntrys();
                        }
                    }
                }
            }

        Run:

            #region Run

            Console.WriteLine();
            Console.WriteLine("---------Ausführung---------");
            Console.Write("Laden....");

            ConsoleSpiner spin = new ConsoleSpiner();
            var ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            _ = Task.Run(() =>
                {
                    while (true)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            break;
                        }
                        spin.Turn();
                    }
                }, ct);

            await RunProcedurAsync();

            ts.Cancel();
            Console.WriteLine();
            Console.WriteLine("++Abgeschlossen++");

            #endregion Run
        }

        /// <summary>
        /// Runs the procedur.
        /// </summary>
        protected override async Task RunProcedurAsync()
        {
            if (!await CheckWorkspaceAsync())
                return;

            if (!await CheckRunnableAsync())
                return;

            Processes = new ObservableCollection<Process>();
            SecondTab = true; // auf zweiten tab switchen
            ProcessSucess = false;
            ProcessError = false;

            ProcessLoadingAnimation = true;
            await RaisePropertyChanged(() => ProcessLoadingAnimation);

            BindingOperations.EnableCollectionSynchronization(Processes, _lock);

            await base.RunProcedurAsync();

            ProcessLoadingAnimation = false;
            await RaisePropertyChanged(() => ProcessLoadingAnimation);
        }

        /// <summary>
        /// Checks the runnable.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckRunnableAsync()
        {
            if (Target == null)
            {
                var view = new DialogSnackbarView();
                view.MySnackbar.MessageQueue.Enqueue("Bitte Target auswählen");
                await DialogHost.Show(view, "DialogSnackbar");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks the workspace.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckWorkspaceAsync()
        {
            if (Workspace == null)
            {
                if (Utils.IsWindowOpen<Window>("MainWindow"))
                {
                    var view = new DialogSnackbarView();
                    view.MySnackbar.MessageQueue.Enqueue("Bitte Workspace auswählen");
                    await DialogHost.Show(view, "DialogSnackbar");
                }

                return false;
            }
            else
            {
                if (Workspace.PBVersion == null)
                {
                    if (Utils.IsWindowOpen<Window>("MainWindow"))
                    {
                        var view = new DialogSnackbarView();
                        view.MySnackbar.MessageQueue.Enqueue("Bitte Powerbuilder-Version angeben");
                        await DialogHost.Show(view, "DialogSnackbar");
                    }

                    return false;
                }
            }

            return true;
        }

        #endregion Methods
    }
}