// project=LibBuilder.WPFCore, file=ProcessHistoryViewModel.cs, creation=2020:8:24
// Copyright (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace LibBuilder.WPFCore.ViewModels
{
    /// <summary>
    /// ProcessHistoryViewModel.
    /// </summary>
    /// <seealso cref="LibBuilder.Core.ViewModels.ProcessHistoryViewModel" />
    public class ProcessHistoryViewModel : Core.ViewModels.ProcessHistoryViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessHistoryViewModel" />
        /// class.
        /// </summary>
        /// <param name="logProvider">The log provider.</param>
        /// <param name="navigationService">The navigation service.</param>
        public ProcessHistoryViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
          : base(logProvider, navigationService)
        {
        }
    }
}