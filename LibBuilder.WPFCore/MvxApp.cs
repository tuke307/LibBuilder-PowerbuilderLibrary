// project=LibBuilder.WPFCore, file=App.xaml.cs, creation=2020:7:21 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
using CommandLine;
using CommandLine.Text;
using Data;
using LibBuilder.Core;
using LibBuilder.WPFCore.Business;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LibBuilder.WPFCore
{
    public class MvxApp : MvxApplication
    {
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

        public override void Initialize()
        {
            Constants.DatabasePath = Path.Combine(Constants.FileDirectory, Data.Constants.DatabaseName);

            if (!Directory.Exists(Constants.FileDirectory))
            {
                Directory.CreateDirectory(Constants.FileDirectory);
            }

            this.RegisterAppStart<WPFCore.ViewModels.MainViewModel>();

            base.Initialize();
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

            if (!options.Application.HasValue)
            {
                Console.WriteLine("Start über Kommandozeile");

                string args = String.Concat(Environment.GetCommandLineArgs());
                string app = "LibBuilder.Console.exe";
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

                // schließen der WPF-App
                Application.Current.Shutdown();
            }
            //new WPFCore.ViewModels.ProcessMainViewModel().Prepare(parameter: options);
        }

        public override Task Startup()
        {
            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.Count() > 0 && arguments != null)
            {
                //var parser = new Parser(with => with.EnableDashDash = true);
                //var result = parser.ParseArguments<Options>(e.Args);
                Core.Utils.AttachConsole(-1);

                var result = Parser.Default.ParseArguments<Options>(arguments)
               .WithParsed(this.RunOptions)
               .WithNotParsed(this.HandleParseError);
            }

            return base.Startup();
        }
    }
}