// project=LibBuilder.Core, file=MainWindowViewModel.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace LibBuilder.Core.ViewModels
{
    public class MainWindowViewModel : MvxViewModel
    {
        private bool _contentVis;

        private object _homeContent;

        private bool _processesVis;

        private bool _settingsVis;

        public bool ContentVis
        {
            get => _contentVis;
            set => SetProperty(ref _contentVis, value);
        }

        public object HomeContent
        {
            get => _homeContent;
            set => SetProperty(ref _homeContent, value);
        }

        public IMvxCommand OpenContentCommand { get; set; }

        public IMvxCommand OpenProcessesCommand { get; set; }

        public IMvxCommand OpenSettingsCommand { get; set; }

        public bool ProcessesVis
        {
            get => _processesVis;
            set => SetProperty(ref _processesVis, value);
        }

        public bool SettingsVis
        {
            get => _settingsVis;
            set => SetProperty(ref _settingsVis, value);
        }

        public MainWindowViewModel()
        {
        }

        public override Task Initialize()
        {
            return base.Initialize();
        }

        public override void Prepare()
        {
            base.Prepare();
        }
    }
}