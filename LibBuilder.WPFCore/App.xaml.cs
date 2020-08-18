// project=LibBuilder.WPFCore, file=App.xaml.cs, creation=2020:7:21 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
using CommandLine;
using CommandLine.Text;
using Data;
using LibBuilder.WPFCore.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace LibBuilder.WPFCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);

        private static void HandleErrors(IEnumerable<Error> errs)
        {
            Console.WriteLine("Fehler beim einlesen der Parameter");
        }

        private static void Run(Options options)
        {
            Console.WriteLine(HeadingInfo.Default);

            if (options.Application.Value)
            {
                Console.WriteLine("Start mit Fenster");
                new MainWindow(options).Show();
            }
            else
            {
                new ViewModels.ContentViewModel(parameter: options);
            }
        }

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
               .WithParsed(Run)
               .WithNotParsed(errs => HandleErrors(errs));
            }
            else
            {
                // normaler start
                new MainWindow().Show();
            }
        }
    }
}