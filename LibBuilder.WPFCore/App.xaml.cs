// project=LibBuilder.WPFCore, file=App.xaml.cs, creation=2020:7:21 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore
{
    using CommandLine;
    using CommandLine.Text;
    using Data;
    using LibBuilder.WPFCore.Business;
    using LibBuilder.WPFCore.Views;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Attaches the console.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);

        /// <summary>
        /// Handles the parse error.
        /// </summary>
        /// <param name="errs">The errs.</param>
        private static void HandleParseError(IEnumerable<Error> errs)
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
        private static void RunOptions(Options options)
        {
            Console.WriteLine("---------LibBuilder-----------");
            Console.WriteLine(HeadingInfo.Default);
            Console.WriteLine(CopyrightInfo.Default);

            if (options.Application.HasValue)
            {
                if (options.Application.Value)
                {
                    // Console.WriteLine("Start mit Fenster");
                    new MainWindow(options).Show();
                }
            }
            else
            {
                new ViewModels.ContentViewModel(parameter: options);
            }
        }

        /// <summary>
        /// Handles the Startup event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        /// The <see cref="StartupEventArgs" /> instance containing the event data.
        /// </param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Constants.DatabasePath = Path.Combine(Constants.FileDirectory, Data.Constants.DatabaseName);

            if (!Directory.Exists(Constants.FileDirectory))
            {
                Directory.CreateDirectory(Constants.FileDirectory);
            }

            if (e.Args.Count() > 0 && e.Args != null)
            {
                //var parser = new Parser(with => with.EnableDashDash = true);
                //var result = parser.ParseArguments<Options>(e.Args);
                AttachConsole(-1);

                var result = Parser.Default.ParseArguments<Options>(e.Args)
               .WithParsed(RunOptions)
               .WithNotParsed(HandleParseError);
            }
            else
            {
                // normaler start
                new MainWindow().Show();
            }
        }
    }
}