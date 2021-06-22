// project=LibBuilder.WPF.Core, file=ApplicationSettingsView.xaml.cs, create=09:16
// Copyright (c) 2021 tuke productions. All rights reserved.
namespace LibBuilder.WPF.Core.Views
{
    using LibBuilder.WPF.Core.Region;
    using LibBuilder.WPF.Core.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;

    /// <summary>
    /// ApplicationSettingsView.
    /// </summary>
    [MvxWpfPresenter("MainWindowRegion", mvxViewPosition.NewOrExsist)]
    public partial class ApplicationSettingsView : MvxWpfView<ApplicationSettingsViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsView" />
        /// class.
        /// </summary>
        public ApplicationSettingsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationSettings.Default.Save();
        }
    }
}