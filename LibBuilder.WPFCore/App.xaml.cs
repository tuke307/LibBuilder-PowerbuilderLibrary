// project=LibBuilder.WPFCore, file=App.xaml.cs, creation=2020:7:21 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
using Data;
using LibBuilder.Core.ViewModels;
using LibBuilder.WPFCore.Views;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace LibBuilder.WPFCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Dictionary<string, string> parameter = new Dictionary<string, string>();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Constants.DatabasePath = Path.Combine(Constants.FileDirectory, Data.Constants.DatabaseName);

            if (!Directory.Exists(Constants.FileDirectory))
                Directory.CreateDirectory(Constants.FileDirectory);

            var args = e.Args;
            if (args != null && args.Count() > 0)
            {
                // einlesen der Parameter
                for (int index = 0; index < args.Length; index += 2)
                {
                    parameter.Add(args[index], args[index + 1]);
                }

                if (parameter.ContainsKey("-a"))
                {
                    new MainWindow(parameter).Show();
                    return;
                }
                else
                {
                    new ViewModels.ContentViewModel(parameter: parameter);
                    return;
                }
            }

            // normaler start
            new MainWindow().Show();
        }
    }
}