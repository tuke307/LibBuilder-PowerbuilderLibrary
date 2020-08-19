using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibBuilder.WPFCore.Business
{
    /// <summary>
    /// Utils.
    /// </summary>
    public static class Utils
    {
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