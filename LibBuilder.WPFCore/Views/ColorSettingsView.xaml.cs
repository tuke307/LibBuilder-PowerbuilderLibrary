// project=LibBuilder.WPFCore, file=Aussehen.xaml.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using LibBuilder.WPFCore.Region;
using LibBuilder.WPFCore.ViewModels;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows.Controls;

namespace LibBuilder.WPFCore.Views
{
    [MvxWpfPresenter("MainWindowRegion", mvxViewPosition.NewOrExsist)]
    public partial class ColorSettingsView : MvxWpfView<ColorSettingsViewModel>
    {
        public ColorSettingsView()
        {
            InitializeComponent();
        }
    }
}