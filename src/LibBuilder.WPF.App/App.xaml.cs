// project=LibBuilder.WPF.App, file=App.xaml.cs, create=09:16 Copyright (c) 2021 Timeline
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
            base.RegisterSetup();
            this.RegisterSetupType<MvxWpfSetup<MvxApp>>();
        }
    }
}