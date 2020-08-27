// project=LibBuilder.WPFCore, file=App.xaml.cs, creation=2020:7:21 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore
{
    using CommandLine;
    using CommandLine.Text;
    using Data;
    using LibBuilder.WPFCore.Business;
    using LibBuilder.WPFCore.Region;
    using LibBuilder.WPFCore.Views;
    using MvvmCross.Core;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterSetup()
        {
            base.RegisterSetup();
            this.RegisterSetupType<MvxWpfSetup<MvxApp>>();
        }
    }
}