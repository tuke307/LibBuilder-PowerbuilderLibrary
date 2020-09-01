using ConsoleTables;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibBuilder.Core.Con.ViewModels
{
    public class OngoingProcessViewModel : Core.ViewModels.OngoingProcessViewModel
    {
        public OngoingProcessViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public new async Task RunProcedurAsync()
        {
            #region Run

            Console.WriteLine();
            Console.WriteLine("---------Ausführung---------");
            Console.Write("Laden....");

            ConsoleSpiner spin = new ConsoleSpiner();
            var ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            _ = Task.Run(() =>
            {
                while (true)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }
                    spin.Turn();
                }
            }, ct);

            await base.RunProcedurAsync();

            ts.Cancel();

            ConsoleTable
       .From<Data.Models.Process>(Processes)
       .Configure(o => o.NumberAlignment = Alignment.Right)
       .Write(Format.Alternative);

            Console.WriteLine();
            Console.WriteLine("++Abgeschlossen++");

            #endregion Run
        }
    }
}