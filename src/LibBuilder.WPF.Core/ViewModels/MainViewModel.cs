// project=LibBuilder.WPF.Core, file=MainViewModel.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.ViewModels
{
    using LibBuilder.WPF.Core.Business;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using System.Threading.Tasks;

    /// <summary>
    /// MainViewModel.
    /// </summary>
    /// <seealso cref="LibBuilder.Core.ViewModels.MainViewModel" />
    public class MainViewModel : LibBuilder.Core.ViewModels.MainViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        /// <param name="logProvider">The log provider.</param>
        /// <param name="navigationService">The navigation service.</param>
        public MainViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
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

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns></returns>
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

        #endregion Methods
    }
}