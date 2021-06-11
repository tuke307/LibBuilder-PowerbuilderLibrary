// project=LibBuilder.WPF.Core, file=MainWindow.xaml.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.Views
{
    using AutoUpdaterDotNET;
    using LibBuilder.WPF.Core.Business;
    using LibBuilder.WPF.Core.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            foreach (var path in ApplicationSettings.Default.UpdatePath.Split(';', ','))
            {
                if (File.Exists(path))
                {
                    AutoUpdater.Start(path);
                    break;
                }
            }
        }

        private void GridTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void WindowClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void WindowMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) { this.WindowState = WindowState.Normal; }
            else if (this.WindowState == WindowState.Normal) { this.WindowState = WindowState.Maximized; }
        }

        private void WindowMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}