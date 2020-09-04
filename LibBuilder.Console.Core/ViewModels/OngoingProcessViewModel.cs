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

namespace LibBuilder.Console.Core.ViewModels
{
    public class OngoingProcessViewModel : LibBuilder.Core.ViewModels.OngoingProcessViewModel
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

            System.Console.WriteLine();
            System.Console.WriteLine("---------Ausführung---------");
            System.Console.WriteLine();

            // Console Tabel creation
            List<string> columns = new List<string>();
            foreach (var item in typeof(Data.Models.Process).GetProperties())
            {
                columns.Add(item.Name);
            }
            processTable = new ConsoleTable(new ConsoleTableOptions() { Columns = columns.ToArray(), EnableCount = false });

            //Proess  subscription
            tableLineStart = System.Console.CursorTop;
            Processes = new ObservableCollection<Process>();
            Processes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CollectionChangedMethod);

            await base.RunProcedurAsync();

            System.Console.WriteLine();
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.WriteLine("++Abgeschlossen++");
            System.Console.ResetColor();

            #endregion Run
        }

        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                System.Console.SetCursorPosition(0, tableLineStart);

                var row = Processes.Last();
                object[] rowArray = new object[] { row.Target, row.Library, row.Object, row.Mode, row.Result };
                processTable.AddRow(rowArray).Write();
            }
        }
    }
}