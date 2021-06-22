// project=LibBuilder.Core, file=ProcessHistoryViewModel.cs, create=09:16 Copyright (c)
// 2021 tuke productions. All rights reserved.
using LibBuilder.Data;
using LibBuilder.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LibBuilder.Core.ViewModels
{
    public class ProcessHistoryViewModel : MvxNavigationViewModel
    {
        private ObservableCollection<ProcessModel> _processes;

        public IMvxCommand ClearProcessesCommand { get; set; }

        public ObservableCollection<ProcessModel> Processes
        {
            get => _processes;
            set => SetProperty(ref _processes, value);
        }

        public ProcessHistoryViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            Log.LogInformation("---START Initialize ProcessHistoryViewModel---");

            ClearProcessesCommand = new MvxCommand(ClearProcesses);

            using (var db = new DatabaseContext())
            {
                //Workspace Liste laden
                Processes = new ObservableCollection<ProcessModel>(db.Process.Include(p => p.Target).ToList());
            }

            Log.LogInformation("---END Initialize ProcessHistoryViewModel---");
        }

        public override Task Initialize()
        {
            return base.Initialize();
        }

        public override void Prepare()
        {
            base.Prepare();
        }

        private void ClearProcesses()
        {
            Log.LogInformation("---START ClearProcesses---");

            using (var db = new DatabaseContext())
            {
                db.Process.RemoveRange(Processes);
                db.SaveChanges();
            }

            Processes.Clear();

            Log.LogInformation("---END ClearProcesses---");
        }
    }
}