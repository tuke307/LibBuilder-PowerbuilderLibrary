using Microsoft.Win32;
using MvvmCross.Commands;
using System.Threading.Tasks;

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

            //UI bezogen
            OpenWorkspaceCommand = new MvxCommand(OpenWorkspace);

            RunCommand = new MvxAsyncCommand(async () => await RunAsync());
        }

        protected void OpenWorkspace()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Powerbuilder Workspace (*.pbw)|*.pbw"
            };

            base.OpenWorkspace(dialog.FileName);
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

        protected new async Task RunAsync()
        {
            if (!CheckWorkspace())
                return;

            if (!CheckRunnable())
                return;

            await base.RunAsync();
        }

        protected override async Task LoadWorkspace()
        {
            if (!CheckWorkspace())
                return;

            await base.LoadWorkspace();
        }
    }
}