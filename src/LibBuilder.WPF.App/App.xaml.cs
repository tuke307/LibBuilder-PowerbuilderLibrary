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