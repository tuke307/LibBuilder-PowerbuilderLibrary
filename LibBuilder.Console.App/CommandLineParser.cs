using CommandLine;
using CommandLine.Text;
using Data;
using LibBuilder.Console.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibBuilder.Console.App
{
    public class CommandLineParser
    {
        private LibBuilder.Console.Core.ViewModels.ProcessSettingsViewModel processSettingsViewModel;

        public CommandLineParser(string[] arguments)
        {
            if (arguments.Count() > 0 && arguments != null)
            {
                Constants.DatabasePath = Path.Combine(Constants.FileDirectory, Data.Constants.DatabaseName);

                using (var db = new DatabaseContext())
                {
                    db.Database.Migrate();
                }

                if (!Directory.Exists(Constants.FileDirectory))
                {
                    Directory.CreateDirectory(Constants.FileDirectory);
                }

                processSettingsViewModel = new LibBuilder.Console.Core.ViewModels.ProcessSettingsViewModel(null, null);
                processSettingsViewModel.Prepare();
                processSettingsViewModel.Initialize();

                var result = Parser.Default.ParseArguments<LibBuilder.Console.Core.Options>(arguments)
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
                System.Console.WriteLine("Fehler beim einlesen der Parameter; ");
                System.Console.Write("{0} Fehler gefunden", errs.Count());
            }
        }

        /// <summary>
        /// Runs the options.
        /// </summary>
        /// <param name="options">The options.</param>
        private void RunOptions(Options options)
        {
            System.Console.WriteLine("---------LibBuilder-----------");
            System.Console.WriteLine(HeadingInfo.Default);
            System.Console.WriteLine(CopyrightInfo.Default);
            System.Console.WriteLine();

            processSettingsViewModel.ParameterStart(options);

            System.Console.ReadKey();
            //new WPFCore.ViewModels.ProcessMainViewModel().Prepare(parameter: options);
        }
    }
}