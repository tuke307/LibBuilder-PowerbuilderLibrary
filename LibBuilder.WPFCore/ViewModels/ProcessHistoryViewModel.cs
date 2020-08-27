using MvvmCross.Logging;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibBuilder.WPFCore.ViewModels
{
    public class ProcessHistoryViewModel : Core.ViewModels.ProcessHistoryViewModel
    {
        public ProcessHistoryViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
          : base(logProvider, navigationService)
        {
        }
    }
}