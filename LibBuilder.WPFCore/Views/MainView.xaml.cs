using LibBuilder.WPFCore.Region;
using LibBuilder.WPFCore.ViewModels;
using MvvmCross.Platforms.Wpf.Views;

namespace LibBuilder.WPFCore.Views
{
    [MvxWpfPresenter("NavigationRegion", mvxViewPosition.NewOrExsist)]
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : MvxWpfView<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();
        }
    }
}