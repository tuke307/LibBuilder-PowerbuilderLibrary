// project=LibBuilder.WPFCore, file=MainWindowViewModel.cs, creation=2020:7:21 Copyright
// (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using LibBuilder.WPFCore.Business;
using LibBuilder.WPFCore.Views;
using MaterialDesignThemes.Wpf;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibBuilder.WPFCore.ViewModels
{
    public class MainViewModel : LibBuilder.Core.ViewModels.MainViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public MainViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            this._navigationService = navigationService;

            OpenSettingsCommand = new MvxCommand(OpenSettings);
            OpenContentCommand = new MvxCommand(OpenContant);
            OpenColorsCommand = new MvxCommand(OpenColors);
            OpenProcessesCommand = new MvxCommand(OpenProcesses);

            //für start
            OpenContant();
        }

        public override Task Initialize()
        {
            ApplicationChanges.LoadColors();

            return base.Initialize();
        }

        public override void Prepare()
        {
            base.Prepare();
        }

        private void OpenColors()
        {
            _navigationService.Navigate<ColorSettingsViewModel>();

            MenuVis = false;
            ContentVis = true;
        }

        private void OpenContant()
        {
            _navigationService.Navigate<ProcessMainViewModel>();

            MenuVis = true;
            ContentVis = false;
        }

        private void OpenProcesses()
        {
            _navigationService.Navigate<ProcessHistoryViewModel>();

            MenuVis = false;
            ContentVis = true;
        }

        private void OpenSettings()
        {
            _navigationService.Navigate<ApplicationSettingsViewModel>();

            MenuVis = false;
            ContentVis = true;
        }
    }
}