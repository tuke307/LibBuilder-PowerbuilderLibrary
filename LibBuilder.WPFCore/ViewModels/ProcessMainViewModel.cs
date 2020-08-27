// project=LibBuilder.WPFCore, file=ContentViewModel.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore.ViewModels
{
    using Data.Models;
    using LibBuilder.WPFCore.Business;
    using LibBuilder.WPFCore.Views;
    using MaterialDesignThemes.Wpf;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;

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
        public ProcessMainViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
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

        public override void ViewAppearing()
        {
            this._navigationService.Navigate<ProcessSettingsViewModel>();
            //this._navigationService.Navigate<OngoingProcessViewModel>();

            this.ProcessTabIndex = 0;
        }

        #endregion Methods
    }
}