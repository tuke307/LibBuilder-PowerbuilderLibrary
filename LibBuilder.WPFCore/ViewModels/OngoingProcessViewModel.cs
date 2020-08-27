using Data.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LibBuilder.WPFCore.ViewModels
{
    public class OngoingProcessViewModel : Core.ViewModels.OngoingProcessViewModel
    {
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
            //if (Target != null)
            //{
            //    RunProcedurCommand.Execute();
            //}

            return base.Initialize();
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