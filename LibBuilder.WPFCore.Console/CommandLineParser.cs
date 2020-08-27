using CommandLine;
using CommandLine.Text;
using Data;
using LibBuilder.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibBuilder.Core.Con
{
    public class CommandLineParser
    {
        private readonly Con.ViewModels.ProcessSettingsViewModel processMainViewModel;

        public CommandLineParser(string[] arguments)
        {
            if (arguments.Count() > 0 && arguments != null)
            {
                Constants.DatabasePath = Path.Combine(Constants.FileDirectory, Data.Constants.DatabaseName);

                if (!Directory.Exists(Constants.FileDirectory))
                {
                    Directory.CreateDirectory(Constants.FileDirectory);
                }

                processMainViewModel = new Con.ViewModels.ProcessSettingsViewModel(null, null);

                var result = Parser.Default.ParseArguments<Options>(arguments)
               .WithParsed(this.RunOptions)
               .WithNotParsed(this.HandleParseError);
            }
        }

        /// <summary>
        /// Handles the parse error.
        /// </summary>
        /// <param name="errs">The errs.</param>
        public void HandleParseError(IEnumerable<Error> errs)
        {
            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
            {
                // kein Fehler, da --help oder --version
            }
            else
            {
                Console.WriteLine("Fehler beim einlesen der Parameter; ");
                Console.Write("{0} Fehler gefunden", errs.Count());
            }
        }

        /// <summary>
        /// Runs the options.
        /// </summary>
        /// <param name="options">The options.</param>
        public void RunOptions(Options options)
        {
            Console.WriteLine("---------LibBuilder-----------");
            Console.WriteLine(HeadingInfo.Default);
            Console.WriteLine(CopyrightInfo.Default);
            Console.WriteLine();

            // normale Applikation starten wenn Parameter "-a" auf "true"
            if (options.Application.HasValue)
            {
                if (options.Application.Value)
                {
                    Console.WriteLine("Start mit Fenster");

                    string args = String.Concat(Environment.GetCommandLineArgs());
                    string app = "LibBuilder.exe";
                    Process runProg = new Process();
                    try
                    {
                        runProg.StartInfo.FileName = app;
                        runProg.StartInfo.Arguments = args;
                        runProg.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not start program " + ex);
                    }
                }
            }

            Task.Run(() => processMainViewModel.ParameterStartAsync(options));
            //new WPFCore.ViewModels.ProcessMainViewModel().Prepare(parameter: options);
        }
    }
}