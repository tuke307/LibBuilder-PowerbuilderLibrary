// project=LibBuilder.WPFCore, file=ProcessSettingsViewModel.cs, creation=2020:8:24
// Copyright (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore.ViewModels
{
    using CommandLine;
    using CommandLine.Text;
    using Data;
    using Data.Models;
    using LibBuilder.Core;
    using LibBuilder.WPFCore.Views;
    using MaterialDesignThemes.Wpf;
    using Microsoft.Win32;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// ProcessSettingsViewModel.
    /// </summary>
    /// <seealso cref="LibBuilder.Core.ViewModels.ProcessSettingsViewModel" />
    public class ProcessSettingsViewModel : LibBuilder.Core.ViewModels.ProcessSettingsViewModel
    {
        /// <summary>
        /// ProcessSettingsViewModel.
        /// </summary>
        /// <param name="logProvider"></param>
        /// <param name="navigationService"></param>
        public ProcessSettingsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            this._navigationService = navigationService;

            WorkspaceSelectedCommand = new MvxAsyncCommand(LoadWorkspace);
            TargetSelectedCommand = new MvxAsyncCommand(LoadTarget);
            LibrarySelectedCommand = new MvxAsyncCommand(LoadLibrary);

            OpenWorkspaceCommand = new MvxAsyncCommand(OpenWorkspace);

            RunProcedurCommand = new MvxAsyncCommand(RunProcedurAsync);

            dialogView = new DialogSnackbarView();
        }

        #region Methods

        /// <summary>
        /// Handles the parse error.
        /// </summary>
        /// <param name="errs">The errs.</param>
        public void HandleParseError(IEnumerable<Error> errs)
        {
            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
            {
                // kein Fehler, da --help oder --version
            }
            else
            {
                Console.WriteLine("Fehler beim einlesen der Parameter; ");
                Console.Write("{0} Fehler gefunden", errs.Count());
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>Initilisierung.</returns>
        public override Task Initialize()
        {
            return base.Initialize();
        }

        /// <summary>
        /// Prepares this instance.
        /// </summary>
        public override void Prepare()
        {
            base.Prepare();

            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.Count() > 0 && arguments != null)
            {
                // Konsole von der der Befehl ausgeführt wird verwenden
                Core.Utils.AttachConsole(-1);

                var result = Parser.Default.ParseArguments<Options>(arguments)
               .WithParsed(this.RunOptions)
               .WithNotParsed(this.HandleParseError);
            }
        }

        /// <summary>
        /// Runs the options.
        /// </summary>
        /// <param name="options">The options.</param>
        public void RunOptions(Options options)
        {
            Console.WriteLine("---------LibBuilder-----------");
            Console.WriteLine(HeadingInfo.Default);
            Console.WriteLine(CopyrightInfo.Default);
            Console.WriteLine();

            Task.Run(() => ParameterStartAsync(options));
            //new WPFCore.ViewModels.ProcessMainViewModel().Prepare(parameter: options);
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

            if (ApplicationSettings.Default.ApplicationRebuildOptions == false)
            {
                using (var db = new DatabaseContext())
                {
                    //Track Entitiy
                    Target.ApplicationRebuild = null;

                    db.Target.Update(Target);
                }
            }

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
        protected async Task ParameterStartAsync(Options parameter)
        {
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
        protected async Task RunProcedurAsync()
        {
            if (!CheckWorkspace())
                return;

            if (!CheckRunnable())
                return;

            // navigate to ongoing and fire runprocedur!!!
            await _navigationService.Navigate<OngoingProcessViewModel, TargetModel>(Target);
        }

        /// <summary>
        /// Checks the runnable.
        /// </summary>
        /// <returns></returns>
        private bool CheckRunnable()
        {
            if (Target == null)
            {
                dialogView.MySnackbar.MessageQueue.Enqueue("Bitte Target auswählen");
                _ = DialogHost.Show(dialogView, "DialogSnackbar");

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
                dialogView.MySnackbar.MessageQueue.Enqueue("Bitte Workspace auswählen");
                _ = DialogHost.Show(dialogView, "DialogSnackbar");

                return false;
            }

            if (Workspace.PBVersion == null)
            {
                dialogView.MySnackbar.MessageQueue.Enqueue("Bitte Powerbuilder-Version angeben");
                _ = DialogHost.Show(dialogView, "DialogSnackbar");

                return false;
            }

            return true;
        }

        #endregion Methods

        #region Properties

        private readonly IMvxNavigationService _navigationService;
        private readonly DialogSnackbarView dialogView;

        #endregion Properties
    }
}