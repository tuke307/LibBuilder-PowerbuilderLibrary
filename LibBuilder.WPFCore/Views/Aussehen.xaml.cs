// project=LibBuilder.WPFCore, file=Aussehen.xaml.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using System.Windows.Controls;

namespace LibBuilder.WPFCore.Views
{
    public partial class Aussehen : UserControl
    {
        public Aussehen()
        {
            InitializeComponent();

            DataContext = new LibBuilder.WPFCore.ViewModels.AussehenViewModel();
        }
    }
}