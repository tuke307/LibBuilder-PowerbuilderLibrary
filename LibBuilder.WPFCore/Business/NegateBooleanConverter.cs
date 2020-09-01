// project=LibBuilder.WPFCore, file=NegateBooleanConverter.cs, creation=2020:8:25
// Copyright (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace LibBuilder.WPFCore.Business
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