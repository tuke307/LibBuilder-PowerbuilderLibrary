// project=LibBuilder.Console.App, file=Program.cs, create=09:16 Copyright (c) 2021
// Timeline Financials GmbH & Co. KG. All rights reserved.
using LibBuilder.Console.Core;

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
            new ConsoleProgram(arguments);
        }
    }
}