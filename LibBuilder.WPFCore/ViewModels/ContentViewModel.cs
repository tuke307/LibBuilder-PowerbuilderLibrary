using Data.Models;
using Microsoft.Win32;
using MvvmCross.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LibBuilder.WPFCore.ViewModels
{
    public class ContentViewModel : LibBuilder.Core.ViewModels.ContentViewModel
    {
        protected MainWindowViewModel mainWindowViewModel;

        public ContentViewModel(MainWindowViewModel mainWindowViewModel)
        {
            //für Snackbar Message
            this.mainWindowViewModel = mainWindowViewModel;

            //Laden
            WorkspaceSelectedCommand = new MvxAsyncCommand(async () => await LoadWorkspace());
            TargetSelectedCommand = new MvxAsyncCommand(async () => await LoadTarget());
            LibrarySelectedCommand = new MvxAsyncCommand(async () => await LoadLibrary());

            //UI bezogen
            OpenWorkspaceCommand = new MvxCommand(OpenWorkspace);

            RunProcedurCommand = new MvxCommand(RunProcedur);

            //letzten modifizierten Workspace laden, mit zuletzt ausgewähltem Target
            //if (Workspaces != null && Workspaces.Count > 0)
            //    Workspace = Workspaces.OrderByDescending(w => w.UpdatedDate).FirstOrDefault();
        }

        protected void OpenWorkspace()
        {
            ContentLoadingAnimation = true;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Powerbuilder Workspace (*.pbw)|*.pbw";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog().HasValue)
            {
                base.OpenWorkspace(openFileDialog.FileName);
            }

            ContentLoadingAnimation = false;
        }

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

        private bool CheckRunnable()
        {
            if (Target == null)
            {
                mainWindowViewModel.NotificationSnackbar.Enqueue("Bitte Target auswählen");
                return false;
            }

            return true;
        }

        protected void RunProcedur()
        {
            if (!CheckWorkspace())
                return;

            if (!CheckRunnable())
                return;

            Processes = new ObservableCollection<Process>();
            SecondTab = true; // auf zweiten tab switchen
            ProcessLoadingAnimation = true;
            ProcessSucess = false;
            ProcessError = false;

            object _lock = new object();
            BindingOperations.EnableCollectionSynchronization(Processes, _lock);

            base.RunProcedur(_lock);
        }

        protected override async Task LoadWorkspace()
        {
            if (!CheckWorkspace())
                return;

            ContentLoadingAnimation = true;

            await base.LoadWorkspace();

            ContentLoadingAnimation = false;
        }

        protected override async Task LoadLibrary()
        {
            ContentLoadingAnimation = true;

            await base.LoadLibrary();

            ContentLoadingAnimation = false;
        }

        protected override async Task LoadTarget()
        {
            ContentLoadingAnimation = true;

            await base.LoadTarget();

            ContentLoadingAnimation = false;
        }
    }
}