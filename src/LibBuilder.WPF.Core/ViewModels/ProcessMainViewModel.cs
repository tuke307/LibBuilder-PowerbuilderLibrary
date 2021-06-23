// project=LibBuilder.WPF.Core, file=ProcessMainViewModel.cs, create=09:16 Copyright (c)
// 2021 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using System.Threading.Tasks;

    /// <summary>
    /// ContentViewModel.
    /// </summary>
    /// <seealso cref="LibBuilder.Core.ViewModels.ProcessMainViewModel" />
    public class ProcessMainViewModel : LibBuilder.Core.ViewModels.ProcessMainViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuslassMainViewModel" /> class.
        /// </summary>
        /// <param name="logProvider">The log provider.</param>
        /// <param name="navigationService">The navigation service.</param>
        public ProcessMainViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            this._navigationService = navigationService;

            //letzten modifizierten Workspace laden, mit zuletzt ausgewähltem Target
            //if (Workspaces != null && Workspaces.Count > 0)
            //    Workspace = Workspaces.OrderByDescending(w => w.UpdatedDate).FirstOrDefault();
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
        /// Prepares the specified user.
        /// </summary>
        /// <param name="_user">The user.</param>
        public override void Prepare()
        {
            base.Prepare();
        }

        /// <summary>
        /// Views the appearing.
        /// </summary>
        public override void ViewAppearing()
        {
            this._navigationService.Navigate<ProcessSettingsViewModel>();
            //this._navigationService.Navigate<OngoingProcessViewModel>();

            this.ProcessTabIndex = 0;
        }

        #endregion Methods
    }
}