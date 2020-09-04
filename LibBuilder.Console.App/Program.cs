using System.Linq;

namespace LibBuilder.Console.App
{
    internal class Program
    {
        private static void Main(string[] arguments)
        {
            new CommandLineParser(arguments);
        }
    }
}