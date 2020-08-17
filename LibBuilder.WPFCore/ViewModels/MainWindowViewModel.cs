// project=LibBuilder.WPFCore, file=MainWindowViewModel.cs, creation=2020:7:21 Copyright
// (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using LibBuilder.WPFCore.Business;
using LibBuilder.WPFCore.Views;
using MaterialDesignThemes.Wpf;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibBuilder.WPFCore.ViewModels
{
    public class MainWindowViewModel : LibBuilder.Core.ViewModels.MainWindowViewModel
    {
        private readonly ApplicationChanges settings = new ApplicationChanges();
        private SnackbarMessageQueue _notificationSnackbar;
        private Dictionary<string, string> parameter;

        public SnackbarMessageQueue NotificationSnackbar
        {
            get => _notificationSnackbar;
            set => SetProperty(ref _notificationSnackbar, value);
        }

        public MainWindowViewModel(Dictionary<string, string> parameter = null)
        {
            this.parameter = parameter;
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
            HomeContent = new Content(this, parameter);
            SettingsVis = true;
            ProcessesVis = true;
            ContentVis = false;
        }

        private void OpenProcesses()
        {
            HomeContent = new Processes();
            SettingsVis = false;
            ProcessesVis = false;
            ContentVis = true;
        }

        private void OpenSettings()
        {
            HomeContent = new Aussehen();
            SettingsVis = false;
            ProcessesVis = false;
            ContentVis = true;
        }
    }
}