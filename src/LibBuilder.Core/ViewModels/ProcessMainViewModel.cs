// project=LibBuilder.Core, file=ProcessMainViewModel.cs, create=09:16 Copyright (c) 2021
// Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.Core.ViewModels
{
    using LibBuilder.Data;
    using LibBuilder.Data.Models;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// ContentViewModel.
    /// </summary>
    /// <seealso cref="MvvmCross.ViewModels.MvxViewModel" />
    public class ProcessMainViewModel : MvxNavigationViewModel
    {
        private int _processTabIndex;

        public int ProcessTabIndex
        {
            get => _processTabIndex;
            set => SetProperty(ref _processTabIndex, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMainViewModel" /> class.
        /// </summary>
        public ProcessMainViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
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

        #endregion Methods
    }
}