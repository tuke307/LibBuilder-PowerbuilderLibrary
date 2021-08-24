﻿// project=LibBuilder.WPF.Core, file=App.xaml.cs, create=14:47 Copyright (c) 2021 Timeline
// Financials GmbH & Co. KG. All rights reserved.
using LibBuilder.WPF.Core;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Views;

namespace LibBuilder.WPF.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxWpfSetup>();
        }
    }
}