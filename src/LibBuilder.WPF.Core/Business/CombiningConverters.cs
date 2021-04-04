// project=LibBuilder.WPF.Core, file=CombiningConverters.cs, creation=2020:7:21 Copyright
// (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using System;
using System.Windows.Data;

namespace LibBuilder.WPF.Core.Business
{
    public class CombiningConverter : IValueConverter
    {
        public IValueConverter Converter1 { get; set; }

        public IValueConverter Converter2 { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object convertedValue = Converter1.Convert(value, targetType, parameter, culture);
            return Converter2.Convert(convertedValue, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IValueConverter Members
    }
}