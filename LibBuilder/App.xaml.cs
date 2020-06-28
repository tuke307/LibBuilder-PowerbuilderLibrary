using Data;
using System.IO;
using System.Windows;

namespace LibBuilder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Constants.DatabasePath = Path.Combine(Constants.FileDirectory, Data.Constants.DatabaseName);

            if (!Directory.Exists(Constants.FileDirectory))
                Directory.CreateDirectory(Constants.FileDirectory);
        }
    }
}