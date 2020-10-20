using LibBuilder.Console.Core;

namespace LibBuilder.Console.CoreApp
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
            new ConsoleProgram(arguments);
        }
    }
}