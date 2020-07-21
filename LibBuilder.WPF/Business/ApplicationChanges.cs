using Data;
using System;
using System.Linq;
using System.Windows;

namespace LibBuilder.WPF.Business
{
    public class ApplicationChanges
    {
        public void LoadColors()
        {
            using (var db = new DatabaseContext())
            {
                var settings = db.Settings.ToList().Last(); //zuletzt hinzugefügter Datensatz
                if (settings != null)
                {
                    Uri primary = new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor." + settings.PrimaryColor + ".xaml");
                    NewResourceDictionary(0, primary);

                    Uri accent = new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor." + settings.SecondaryColor + ".xaml");
                    NewResourceDictionary(1, accent);

                    Uri basetheme = new Uri($"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme." + StringBaseTheme(settings.DarkMode) + ".xaml");
                    NewResourceDictionary(2, basetheme);
                }
            }
        }

        public void SetPrimary(object primary_color)
        {
            using (var db = new DatabaseContext())
            {
                var settings = db.Settings.ToList().Last(); //zuletzt hinzugefügter Datensatz
                if (settings != null)
                {
                    Uri primary = new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor." + primary_color.ToString() + ".xaml");
                    NewResourceDictionary(0, primary);

                    settings.PrimaryColor = primary_color.ToString();

                    db.SaveChanges();
                }
            }
        }

        public void SetAccent(object acccent_color)
        {
            using (var db = new DatabaseContext())
            {
                var settings = db.Settings.ToList().Last(); //zuletzt hinzugefügter Datensatz
                if (settings != null)
                {
                    Uri accent = new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor." + acccent_color.ToString() + ".xaml");
                    NewResourceDictionary(1, accent);

                    settings.SecondaryColor = acccent_color.ToString();

                    db.SaveChanges();
                }
            }
        }

        public void SetBaseTheme(object base_color)
        {
            using (var db = new DatabaseContext())
            {
                var settings = db.Settings.ToList().Last(); //zuletzt hinzugefügter Datensatz
                if (settings != null)
                {
                    Uri basetheme = new Uri($"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme." + StringBaseTheme((bool)base_color) + ".xaml");
                    NewResourceDictionary(2, basetheme);

                    settings.DarkMode = (bool)base_color;

                    db.SaveChanges();
                }
            }
        }

        private void NewResourceDictionary(int position, Uri newvalue)
        {
            Application.Current.Resources.MergedDictionaries.RemoveAt(position);
            Application.Current.Resources.MergedDictionaries.Insert(position, new ResourceDictionary() { Source = newvalue });
        }

        private string StringBaseTheme(bool value)
        {
            if (value)
                return "Dark";
            else
                return "Light";
        }
    }
}