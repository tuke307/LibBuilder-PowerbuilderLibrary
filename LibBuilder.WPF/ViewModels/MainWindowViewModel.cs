using LibBuilder.WPF.Business;
using LibBuilder.WPF.Views;
using MaterialDesignThemes.Wpf;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibBuilder.WPF.ViewModels
{
    public class MainWindowViewModel : LibBuilder.Core.ViewModels.MainWindowViewModel
    {
        private readonly ApplicationChanges settings = new ApplicationChanges();

        public MainWindowViewModel()
        {
            OpenSettingsCommand = new MvxCommand(OpenSettings);
            OpenContentCommand = new MvxCommand(OpenContant);
            OpenProcessesCommand = new MvxCommand(OpenProcesses);

            //für start
            OpenContant();

            NotificationSnackbar = new SnackbarMessageQueue();
        }

        public override Task Initialize()
        {
            settings.LoadColors();

            return base.Initialize();
        }

        public override void Prepare()
        {
            base.Prepare();
        }

        private void OpenContant()
        {
            HomeContent = new Content(this);
            SettingsVis = true;
            ProcessesVis = true;
            ContentVis = false;
        }

        private void OpenSettings()
        {
            HomeContent = new Aussehen();
            SettingsVis = false;
            ProcessesVis = false;
            ContentVis = true;
        }

        private void OpenProcesses()
        {
            HomeContent = new Processes();
            SettingsVis = false;
            ProcessesVis = false;
            ContentVis = true;
        }

        private SnackbarMessageQueue _notificationSnackbar;

        public SnackbarMessageQueue NotificationSnackbar
        {
            get => _notificationSnackbar;
            set => SetProperty(ref _notificationSnackbar, value);
        }
    }
}