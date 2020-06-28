using LibBuilder.Business;
using LibBuilder.Views;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;

namespace LibBuilder.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ApplicationChanges settings = new ApplicationChanges();

        public MainWindowViewModel()
        {
            OpenSettingsCommand = new ActionCommand(OpenSettings);
            OpenContentCommand = new ActionCommand(OpenContant);
            OpenProcessesCommand = new ActionCommand(OpenProcesses);

            //für start
            OpenContant(null);

            Application_load();

            NotificationSnackbar = new SnackbarMessageQueue();
        }

        public ICommand OpenSettingsCommand { get; set; }
        public ICommand OpenContentCommand { get; set; }
        public ICommand OpenProcessesCommand { get; set; }

        private void OpenContant(object obj)
        {
            HomeContent = new Content(this);
            SettingsVis = true;
            ProcessesVis = true;
            ContentVis = false;
        }

        private void OpenSettings(object obj)
        {
            HomeContent = new Aussehen();
            SettingsVis = false;
            ProcessesVis = false;
            ContentVis = true;
        }

        private void OpenProcesses(object obj)
        {
            HomeContent = new Processes();
            SettingsVis = false;
            ProcessesVis = false;
            ContentVis = true;
        }

        private void Application_load()
        {
            settings.LoadColors();
        }

        public bool ProcessesVis
        {
            get => Get<bool>();
            set => Set(value);
        }

        public bool SettingsVis
        {
            get => Get<bool>();
            set => Set(value);
        }

        public bool ContentVis
        {
            get => Get<bool>();
            set => Set(value);
        }

        public object HomeContent
        {
            get => Get<object>();
            set => Set(value);
        }

        public SnackbarMessageQueue NotificationSnackbar
        {
            get => Get<SnackbarMessageQueue>();
            set => Set(value);
        }
    }
}