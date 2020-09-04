// project=LibBuilder.WPF.Core, file=ProcessSettingsViewModel.cs, creation=2020:8:24
// Copyright (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.ViewModels
{
    using Data;
    using Data.Models;
    using LibBuilder.WPF.Core.Dialogs;
    using MaterialDesignThemes.Wpf;
    using Microsoft.Win32;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
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
            if (!CheckTarget())
            {
                return;
            }

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
        /// Runs the procedur.
        /// </summary>
        protected async Task RunProcedurAsync()
        {
            if (!CheckWorkspace())
                return;

            if (!CheckTarget())
                return;

            // navigate to ongoing and fire runprocedur!!!
            await _navigationService.Navigate<OngoingProcessViewModel, TargetModel>(Target);
        }

        /// <summary>
        /// Checks the runnable.
        /// </summary>
        /// <returns></returns>
        private bool CheckTarget()
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

            if (WorkspacePBVersion == null)
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