// project=LibBuilder.WPF.Core, file=OngoingProcessViewModel.cs, creation=2020:8:24
// Copyright (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using Data.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LibBuilder.WPF.Core.ViewModels
{
    /// <summary>
    /// OngoingProcessViewModel.
    /// </summary>
    /// <seealso cref="LibBuilder.Core.ViewModels.OngoingProcessViewModel" />
    public class OngoingProcessViewModel : LibBuilder.Core.ViewModels.OngoingProcessViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OngoingProcessViewModel" />
        /// class.
        /// </summary>
        /// <param name="logProvider">The log provider.</param>
        /// <param name="navigationService">The navigation service.</param>
        public OngoingProcessViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            RunProcedurCommand = new MvxAsyncCommand(RunProcedurAsync);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>Initilisierung.</returns>
        public override Task Initialize()
        {
            var task = base.Initialize();

            if (Target != null)
            {
                RunProcedurCommand.Execute();
            }

            return task;
        }

        /// <summary>
        /// Prepares the specified user.
        /// </summary>
        /// <param name="_user">The user.</param>
        public override void Prepare(TargetModel Target = null)
        {
            base.Prepare(Target);
        }

        /// <summary>
        /// Runs the procedur.
        /// </summary>
        protected override async Task RunProcedurAsync()
        {
            Processes = new ObservableCollection<Process>();
            ProcessSucess = false;
            ProcessError = false;

            ProcessLoadingAnimation = true;
            await RaisePropertyChanged(() => ProcessLoadingAnimation);

            BindingOperations.EnableCollectionSynchronization(Processes, _lock);

            await base.RunProcedurAsync();

            ProcessLoadingAnimation = false;
            await RaisePropertyChanged(() => ProcessLoadingAnimation);
        }
    }
}