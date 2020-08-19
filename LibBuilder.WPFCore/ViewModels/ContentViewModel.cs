// project=LibBuilder.WPFCore, file=ContentViewModel.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore.ViewModels
{
    using Data.Models;
    using LibBuilder.WPFCore.Business;
    using LibBuilder.WPFCore.Views;
    using Microsoft.Win32;
    using MvvmCross.Commands;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
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

        /// <summary>
        /// Dotses this instance.
        /// </summary>
        public static void Dots()
        {
            while (true)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.Write('.');
                    System.Threading.Thread.Sleep(1000);
                    if (i == 2)
                    {
                        Console.Write("\r   \r");
                        i = -1;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
        }

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
            if (!CheckWorkspace())
                return;

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
                Workspace = Workspaces.Single(w => w.File.ToLower() == Path.GetFileName(parameter.Workspace).ToLower());

                await LoadWorkspace();

                Console.WriteLine("Workspace " + Workspace.FilePath + " erfolgreich eingelesen");
            }
            else
            {
                // ohne Workspace geht nicht
                return;
            }

            // Target
            if (parameter.Target != null)
            {
                Target = Targets.Single(t => t.File.ToLower() == Path.GetFileName(parameter.Target).ToLower());
                await LoadTarget();
                Console.WriteLine("Target " + Target.FilePath + " erfolgreich eingelesen");
            }
            else
            {
                // zuletzt ausgewähltes Target laden
                Target = Targets.OrderByDescending(t => t.UpdatedDate).FirstOrDefault();
                await LoadTarget();
                Console.WriteLine("Target " + Target.FilePath + " erfolgreich eingelesen");
            }

            // Version
            if (parameter.Version != null)
            {
                // Version parsen
                PBDotNetLib.orca.Orca.Version version = (PBDotNetLib.orca.Orca.Version)Enum.Parse(typeof(PBDotNetLib.orca.Orca.Version), "PB" + parameter.Version);
                Workspace.PBVersion = version;

                await base.SaveWorkspace();
                await LoadWorkspace();

                Console.WriteLine("Version erfolgreich gesetzt");
            }
            else
            {
                // Version, die in DB gespeichert ist
            }

            //Librays
            if (!parameter.Librarys.IsNullOrEmpty())
            {
                base.DeselectAllLibrarys();

                Console.WriteLine("Librarys " + String.Join(", ", parameter.Librarys.ToArray()) + " selektiert");
            }
            else
            {
                // letzte Auswahl nehmen die in DB gespeichert ist
            }

            // Build
            if (parameter.Build.HasValue)
            {
                foreach (var lib in parameter.Librarys)
                {
                    Library = Librarys.Single(l => l.File.ToLower() == Path.GetFileName(lib).ToLower());
                    if (Library != null)
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
                }
            }
            else
            {
                // Letzte Auswahl die in DB gespeichert ist
            }

            // Regenerate
            if (parameter.Regenerate.HasValue)
            {
                foreach (var lib in parameter.Librarys)
                {
                    Library = Librarys.Single(l => l.File.ToLower() == Path.GetFileName(lib).ToLower());
                    if (Library != null)
                    {
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

                        await base.SaveLibrary();
                    }
                }
            }
            else
            {
                // Letzte Auswahl die in DB gespeichert ist
            }

            #region Run

            Console.WriteLine();
            Console.WriteLine("---------Ausführung---------");
            Console.Write("Laden....");

            ConsoleSpiner spin = new ConsoleSpiner();
            Task loadingSpinner = Task.Run(() => spin.Turn());

            await RunProcedurAsync();

            loadingSpinner.Dispose();
            Console.WriteLine();
            Console.WriteLine("++Abgeschlossen++");

            #endregion Run
        }

        /// <summary>
        /// Runs the procedur.
        /// </summary>
        protected override async Task RunProcedurAsync()
        {
            if (!CheckWorkspace())
                return;

            if (!CheckRunnable())
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
        private bool CheckRunnable()
        {
            if (Target == null)
            {
                mainWindowViewModel.NotificationSnackbar.Enqueue("Bitte Target auswählen");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks the workspace.
        /// </summary>
        /// <returns></returns>
        private bool CheckWorkspace()
        {
            if (Workspace == null)
            {
                mainWindowViewModel.NotificationSnackbar.Enqueue("Bitte Workspace auswählen");
                return false;
            }
            else
            {
                if (Workspace.PBVersion == null)
                {
                    mainWindowViewModel.NotificationSnackbar.Enqueue("Bitte Powerbuilder-Version angeben");
                    return false;
                }
            }

            return true;
        }
    }
}