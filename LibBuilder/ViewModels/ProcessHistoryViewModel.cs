using Data;
using Data.Models;
using LibBuilder.Business;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace LibBuilder.ViewModels
{
    public class ProcessHistoryViewModel : BaseViewModel
    {
        public ProcessHistoryViewModel()
        {
            ClearProcessesCommand = new ActionCommand(ClearProcesses);

            using (var db = new DatabaseContext())
            {
                //Workspace Liste laden
                Processes = new ObservableCollection<ProcessModel>(db.Process.Include(p => p.Target).ToList());
            }
        }

        public ICommand ClearProcessesCommand { get; set; }

        private void ClearProcesses(object obj)
        {
            using (var db = new DatabaseContext())
            {
                db.Process.RemoveRange(Processes);
                db.SaveChanges();
            }

            Processes.Clear();
        }

        public ObservableCollection<ProcessModel> Processes
        {
            get => Get<ObservableCollection<ProcessModel>>();
            set => Set(value);
        }
    }
}