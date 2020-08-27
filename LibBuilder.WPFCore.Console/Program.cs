using System.Linq;

namespace LibBuilder.Core.Con
{
    internal class Program
    {
        private static void Main(string[] arguments)
        {
            new CommandLineParser(arguments);
        }
    }
}