// project=LibBuilder.Console.Core, file=CommandLineParser.cs, create=09:16 Copyright (c)
// 2021 Timeline Financials GmbH & Co. KG. All rights reserved.
using CommandLine;
using System.Collections.Generic;

namespace LibBuilder.Console.Core
{
    /// <summary>
    /// CommandLineParser.
    /// </summary>
    public class CommandLineParser
    {
        private readonly LibBuilder.Console.Core.ViewModels.ProcessSettingsViewModel processSettingsViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParser" /> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public CommandLineParser(string[] arguments)
        {
            // Initialize ViewModel
            processSettingsViewModel = new LibBuilder.Console.Core.ViewModels.ProcessSettingsViewModel(null, null);
            processSettingsViewModel.Prepare();
            processSettingsViewModel.Initialize();

            ReadLine(arguments);
        }

        public void ReadLine(string[] arguments = null)
        {
            if (arguments == null)
            {
                arguments = System.Console.ReadLine().Split(' ');
            }

            // Parse Parameters
            var result = Parser.Default.ParseArguments<LibBuilder.Console.Core.Options>(arguments)
           .WithParsed(this.RunOptions)
           .WithNotParsed(this.HandleParseError);
        }

        /// <summary>
        /// Handles the parse error.
        /// </summary>
        /// <param name="errs">The errs.</param>
        private void HandleParseError(IEnumerable<Error> errs)
        {
            //if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
            //{
            //    // kein Fehler, da --help oder --version
            //}
            //else if (errs.Any(x => x is CommandLine.MissingRequiredOptionError || x is CommandLine.MissingValueOptionError || x is CommandLine.MissingGroupOptionError))
            //{
            //    //System.Console.WriteLine("Fehler beim einlesen der Parameter; etwas fehlt!");
            //}
            //else
            //{
            //    //System.Console.WriteLine("Fehler beim einlesen der Parameter; ");
            //    //System.Console.Write("{0} Fehler gefunden", errs.Count());
            //}

            ReadLine();
        }

        /// <summary>
        /// Runs the options.
        /// </summary>
        /// <param name="options">The options.</param>
        private void RunOptions(Options options)
        {
            processSettingsViewModel.ParameterStart(options);

            ReadLine();
        }
    }
}