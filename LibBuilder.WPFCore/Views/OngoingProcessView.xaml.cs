using LibBuilder.WPFCore.Region;
using LibBuilder.WPFCore.ViewModels;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows.Controls;

namespace LibBuilder.WPFCore.Views
{
    [MvxWpfPresenter("ProcessMainRegion", mvxViewPosition.New)]
    /// <summary>
    /// Interaction logic for OngoingProcessView.xaml
    /// </summary>
    public partial class OngoingProcessView : MvxWpfView<OngoingProcessViewModel>
    {
        public OngoingProcessView()
        {
            InitializeComponent();
        }
    }
}