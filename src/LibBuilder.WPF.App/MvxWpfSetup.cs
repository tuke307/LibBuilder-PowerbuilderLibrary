// project=LibBuilder.WPF.Core, file=MvxWpfSetup.cs, creation=2020:8:24 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
using Data;
using LibBuilder.WPF.Core.Region;
using MvvmCross.Logging;
using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.ViewModels;
using Serilog;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;

namespace LibBuilder.WPF.App
{
    public class MvxWpfSetup<TApplication> : MvvmCross.Platforms.Wpf.Core.MvxWpfSetup<TApplication> where TApplication : class, IMvxApplication, new()
    {
        public override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.Serilog;

        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            var list = new List<Assembly>();
            list.AddRange(base.GetViewAssemblies());
            list.Add(typeof(LibBuilder.WPF.Core.Views.MainWindow).Assembly);
            return list.ToArray();
        }

        protected override IMvxLogProvider CreateLogProvider()
        {
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Verbose()
              .WriteTo.RollingFile(Constants.LogPath)
              .CreateLogger();

            return base.CreateLogProvider();
        }

        protected override IMvxWpfViewPresenter CreateViewPresenter(ContentControl root)
        {
            return new MvxWpfPresenter(root);
        }
    }
}