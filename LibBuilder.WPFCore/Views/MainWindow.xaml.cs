// project=LibBuilder.WPFCore, file=MainWindow.xaml.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LibBuilder.WPFCore.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Dictionary<string, string> parameter = null)
        {
            InitializeComponent();

            DataContext = new LibBuilder.WPFCore.ViewModels.MainWindowViewModel(parameter);
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