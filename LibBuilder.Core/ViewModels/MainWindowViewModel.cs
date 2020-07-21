using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace LibBuilder.Core.ViewModels
{
    public class MainWindowViewModel : MvxViewModel
    {
        public MainWindowViewModel()
        {
        }

        public IMvxCommand OpenSettingsCommand { get; set; }
        public IMvxCommand OpenContentCommand { get; set; }
        public IMvxCommand OpenProcessesCommand { get; set; }

        public override Task Initialize()
        {
            return base.Initialize();
        }

        public override void Prepare()
        {
            base.Prepare();
        }

        private bool _processesVis;

        public bool ProcessesVis
        {
            get => _processesVis;
            set => SetProperty(ref _processesVis, value);
        }

        private bool _settingsVis;

        public bool SettingsVis
        {
            get => _settingsVis;
            set => SetProperty(ref _settingsVis, value);
        }

        private bool _contentVis;

        public bool ContentVis
        {
            get => _contentVis;
            set => SetProperty(ref _contentVis, value);
        }

        private object _homeContent;

        public object HomeContent
        {
            get => _homeContent;
            set => SetProperty(ref _homeContent, value);
        }
    }
}