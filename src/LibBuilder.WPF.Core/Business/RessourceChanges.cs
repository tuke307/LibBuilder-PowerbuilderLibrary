// project=LibBuilder.WPF.Core, file=RessourceChanges.cs, creation=2020:8:24 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using System;
using System.Windows;

namespace LibBuilder.WPF.Core.Business
{
    /// <summary>
    /// RessourceChanges.
    /// </summary>
    public class RessourceChanges
    {
        /// <summary>
        /// Colorses the specified primary color.
        /// </summary>
        /// <param name="primaryColor">Color of the primary.</param>
        /// <param name="secondaryColor">Color of the secondary.</param>
        /// <param name="baseTheme">The base theme.</param>
        public void Colors(string primaryColor = null, string secondaryColor = null, string baseTheme = null)
        {
            int position = 0;
            Uri changes = null;

            if (!string.IsNullOrEmpty(primaryColor))
            {
                position = 0;
                changes = new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor." + primaryColor + ".xaml");

                Application.Current.Resources.MergedDictionaries.RemoveAt(position);
                Application.Current.Resources.MergedDictionaries.Insert(position, new ResourceDictionary() { Source = changes });
            }

            if (!string.IsNullOrEmpty(secondaryColor))
            {
                position = 1;
                changes = new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor." + secondaryColor + ".xaml");
                Application.Current.Resources.MergedDictionaries.RemoveAt(position);
                Application.Current.Resources.MergedDictionaries.Insert(position, new ResourceDictionary() { Source = changes });
            }

            if (!string.IsNullOrEmpty(baseTheme))
            {
                position = 2;
                changes = new Uri($"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme." + baseTheme + ".xaml");
                Application.Current.Resources.MergedDictionaries.RemoveAt(position);
                Application.Current.Resources.MergedDictionaries.Insert(position, new ResourceDictionary() { Source = changes });
            }
        }
    }
}