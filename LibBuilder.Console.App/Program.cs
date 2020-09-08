using CommandLine.Text;
using Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace LibBuilder.Console.App
{
    /// <summary>
    /// Program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        private static void Main(string[] arguments)
        {
            System.Console.WriteLine("---------LibBuilder-----------");
            System.Console.WriteLine(HeadingInfo.Default);
            System.Console.WriteLine(CopyrightInfo.Default);
            System.Console.WriteLine();

            // Initializing
            Constants.DatabasePath = Path.Combine(Constants.FileDirectory, Data.Constants.DatabaseName);

            using (var db = new DatabaseContext())
            {
                db.Database.Migrate();
            }

            if (!Directory.Exists(Constants.FileDirectory))
            {
                Directory.CreateDirectory(Constants.FileDirectory);
            }

            if (arguments.Count() > 0 && arguments != null)
            {
                new CommandLineParser(arguments);
            }
            else
            {
                System.Console.WriteLine("Bitte Parameter angeben");
                string[] inputArguments = System.Console.ReadLine().Split(' ');

                new CommandLineParser(inputArguments);
            }
        }
    }
}