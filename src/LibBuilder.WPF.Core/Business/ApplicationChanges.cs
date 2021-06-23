// project=LibBuilder.WPF.Core, file=ApplicationChanges.cs, create=09:16 Copyright (c)
// 2021 Timeline Financials GmbH & Co. KG. All rights reserved.
using MaterialDesignColors;
using System;

namespace LibBuilder.WPF.Core.Business
{
    /// <summary>
    /// ApplicationChanges.
    /// </summary>
    public static class ApplicationChanges
    {
        public static MaterialDesignThemes.Wpf.BaseTheme BoolToBaseTheme(bool value)
        {
            if (value)
                return MaterialDesignThemes.Wpf.BaseTheme.Dark;
            else
                return MaterialDesignThemes.Wpf.BaseTheme.Light;
        }

        public static bool IsDarkTheme()
        {
            if (Color.Default.Theme == MaterialDesignThemes.Wpf.BaseTheme.Dark)
                return true;
            else
                return false;
        }

        public static void LoadColors()
        {
            var appChanges = new RessourceChanges();

            appChanges.Colors(
                primaryColor: Color.Default.Primary.ToString(),
                secondaryColor: Color.Default.Secondary.ToString(),
                baseTheme: Color.Default.Theme.ToString());
        }

        public static void SetAccent(Swatch acccent_color)
        {
            Color.Default.Secondary = (MaterialDesignColors.SecondaryColor)Enum.Parse(typeof(MaterialDesignColors.SecondaryColor), acccent_color.Name);

            Color.Default.Save();

            var appChanges = new RessourceChanges();

            appChanges.Colors(secondaryColor: Color.Default.Secondary.ToString());
        }

        public static void SetBaseTheme(bool base_color)
        {
            Color.Default.Theme = ApplicationChanges.BoolToBaseTheme(base_color);

            Color.Default.Save();

            var appChanges = new RessourceChanges();

            appChanges.Colors(baseTheme: Color.Default.Theme.ToString());
        }

        public static void SetPrimary(Swatch primary_color)
        {
            Color.Default.Primary = (MaterialDesignColors.PrimaryColor)Enum.Parse(typeof(MaterialDesignColors.PrimaryColor), primary_color.ToString(), true);

            Color.Default.Save();

            var appChanges = new RessourceChanges();

            appChanges.Colors(primaryColor: Color.Default.Primary.ToString());
        }
    }
}