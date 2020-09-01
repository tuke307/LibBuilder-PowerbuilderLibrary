// project=LibBuilder.WPFCore, file=MvxWpfSetup.cs, creation=2020:8:24 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.ViewModels;
using System.Windows.Controls;

namespace LibBuilder.WPFCore.Region
{
    public class MvxWpfSetup<TApplication> : MvvmCross.Platforms.Wpf.Core.MvxWpfSetup<TApplication> where TApplication : class, IMvxApplication, new()
    {
        protected override IMvxWpfViewPresenter CreateViewPresenter(ContentControl root)
        {
            return new MvxWpfPresenter(root);
        }
    }
}