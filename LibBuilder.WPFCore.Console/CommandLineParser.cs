using CommandLine;
using CommandLine.Text;
using Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LibBuilder.Core.Con
{
    public class CommandLineParser
    {
        private Con.ViewModels.ProcessSettingsViewModel processSettingsViewModel;

        public CommandLineParser(string[] arguments)
        {
            if (arguments.Count() > 0 && arguments != null)
            {
                Constants.DatabasePath = Path.Combine(Constants.FileDirectory, Data.Constants.DatabaseName);

                if (!Directory.Exists(Constants.FileDirectory))
                {
                    Directory.CreateDirectory(Constants.FileDirectory);
                }

                processSettingsViewModel = new Con.ViewModels.ProcessSettingsViewModel(null, null);
                processSettingsViewModel.Prepare();
                processSettingsViewModel.Initialize();

                var result = Parser.Default.ParseArguments<Options>(arguments)
               .WithParsed(this.RunOptions)
               .WithNotParsed(this.HandleParseError);
            }
        }

        /// <summary>
        /// Handles the parse error.
        /// </summary>
        /// <param name="errs">The errs.</param>
        private void HandleParseError(IEnumerable<Error> errs)
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
        private void RunOptions(Options options)
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

            processSettingsViewModel.ParameterStartAsync(options).Wait();

            Console.ReadKey();
            //new WPFCore.ViewModels.ProcessMainViewModel().Prepare(parameter: options);
        }
    }
}