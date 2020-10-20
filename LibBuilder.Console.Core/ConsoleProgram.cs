using CommandLine.Text;
using Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace LibBuilder.Console.Core
{
    /// <summary>
    /// Program.
    /// </summary>
    public class ConsoleProgram
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public ConsoleProgram(string[] arguments)
        {
            System.Console.WriteLine("---------LibBuilder-----------");
            System.Console.WriteLine(HeadingInfo.Default);
            System.Console.WriteLine(CopyrightInfo.Default);
            System.Console.WriteLine();

            // Initializing
            if (!Directory.Exists(Constants.FileDirectory))
            {
                Directory.CreateDirectory(Constants.FileDirectory);
            }

            using (var db = new DatabaseContext())
            {
                db.Database.Migrate();
            }

            new CommandLineParser(arguments);
        }
    }
}