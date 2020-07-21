using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LibBuilder.Core.ViewModels
{
    public class ProcessHistoryViewModel : MvxViewModel
    {
        public ProcessHistoryViewModel()
        {
            ClearProcessesCommand = new MvxCommand(ClearProcesses);

            using (var db = new DatabaseContext())
            {
                //Workspace Liste laden
                Processes = new ObservableCollection<ProcessModel>(db.Process.Include(p => p.Target).ToList());
            }
        }

        public IMvxCommand ClearProcessesCommand { get; set; }

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
            using (var db = new DatabaseContext())
            {
                db.Process.RemoveRange(Processes);
                db.SaveChanges();
            }

            Processes.Clear();
        }

        private ObservableCollection<ProcessModel> _processes;

        public ObservableCollection<ProcessModel> Processes
        {
            get => _processes;
            set => SetProperty(ref _processes, value);
        }
    }
}