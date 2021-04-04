using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace LibBuilder.Core
{
    /// <summary>
    /// Utils.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Attaches the console.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);

        /// <summary>
        /// Determines whether [is null or empty].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>
        /// <c>true</c> if [is null or empty] [the specified enumerable]; otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}