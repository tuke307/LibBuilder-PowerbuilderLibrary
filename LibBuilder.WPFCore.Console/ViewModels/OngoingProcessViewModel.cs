using ConsoleTables;
using Data.Models;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibBuilder.Core.Con.ViewModels
{
    public class OngoingProcessViewModel : Core.ViewModels.OngoingProcessViewModel
    {
        private ConsoleTable processTable;
        private int tableLineStart;

        public OngoingProcessViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public new async Task RunProcedurAsync()
        {
            #region Run

            Console.WriteLine();
            Console.WriteLine("---------Ausführung---------");
            Console.WriteLine();

            // Console Tabel creation
            List<string> columns = new List<string>();
            foreach (var item in typeof(Data.Models.Process).GetProperties())
            {
                columns.Add(item.Name);
            }
            processTable = new ConsoleTable(new ConsoleTableOptions() { Columns = columns.ToArray(), EnableCount = false });

            //Proess  subscription
            tableLineStart = Console.CursorTop;
            Processes = new ObservableCollection<Process>();
            Processes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CollectionChangedMethod);

            await base.RunProcedurAsync();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("++Abgeschlossen++");
            Console.ResetColor();

            #endregion Run
        }

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Console.SetCursorPosition(0, tableLineStart);

                var row = Processes.Last();
                object[] rowArray = new object[] { row.Target, row.Library, row.Object, row.Mode, row.Result };
                processTable.AddRow(rowArray).Write();
            }
        }
    }
}