// project=LibBuilder.WPFCore, file=App.xaml.cs, creation=2020:7:21 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.

using Data;
using Microsoft.EntityFrameworkCore;
using MvvmCross.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LibBuilder.WPFCore
{
    public class MvxApp : MvxApplication
    {
        public override void Initialize()
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

            this.RegisterAppStart<WPFCore.ViewModels.MainViewModel>();

            base.Initialize();
        }

        public override Task Startup()
        {
            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.Count() > 0 && arguments != null)
            {
                if (arguments[0].ToLower().EndsWith("libbuilder.dll")) return base.Startup();

                Core.Utils.AttachConsole(-1);

                string args = String.Concat(arguments);
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

            return base.Startup();
        }
    }
}