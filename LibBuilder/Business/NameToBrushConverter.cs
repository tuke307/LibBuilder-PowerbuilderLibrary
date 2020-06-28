using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LibBuilder.Business
{
    public class NameToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //string input = value as string;
            switch (value.ToString())
            {
                case "PBORCA_OK":
                    return Brushes.Green;

                case "":
                    return DependencyProperty.UnsetValue;

                default:
                    return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}