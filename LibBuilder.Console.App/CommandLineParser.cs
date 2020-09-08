using CommandLine;
using LibBuilder.Console.Core;
using System.Collections.Generic;
using System.Linq;

namespace LibBuilder.Console.App
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

            // Parse Parameter
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
            processSettingsViewModel.ParameterStart(options);

            System.Console.ReadKey();
            //new WPFCore.ViewModels.ProcessMainViewModel().Prepare(parameter: options);
        }
    }
}