// project=LibBuilder.WPF.Core, file=NegateBooleanConverter.cs, create=09:16 Copyright (c)
// 2021 tuke productions. All rights reserved.
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace LibBuilder.WPF.Core.Business
{
    public class NegateBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}