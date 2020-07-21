using System;
using System.IO;

namespace Data
{
    public class Constants
    {
        public const string DatabaseName = "libbuilder.db";

        public static string DatabasePath { get; set; }


        public static string FileDirectory
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LibBuilder");
            }
        }
    }
}