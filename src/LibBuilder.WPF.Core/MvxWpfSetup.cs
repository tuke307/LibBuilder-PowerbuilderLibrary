// project=LibBuilder.WPF.Core, file=MvxWpfSetup.cs, create=14:46 Copyright (c) 2021
// Timeline Financials GmbH & Co. KG. All rights reserved.
using LibBuilder.Data;
using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.ViewModels;
using Serilog;
using Serilog.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;

namespace LibBuilder.WPF.App
{
    public class MvxWpfSetup : MvvmCross.Platforms.Wpf.Core.MvxWpfSetup<LibBuilder.WPF.Core.MvxApp>
    {
        protected override ILoggerFactory CreateLogFactory()
        {
            // serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(Constants.LogPath, rollingInterval: RollingInterval.Month)
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override IMvxWpfViewPresenter CreateViewPresenter(ContentControl root)
        {
            return new Mvx.Wpf.ItemsPresenter.MvxWpfPresenter(root);
        }
    }
}