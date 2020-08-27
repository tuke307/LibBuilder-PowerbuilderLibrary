using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace LibBuilder.WPFCore.Business
{
    /// <summary>
    /// Utils.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Determines whether [is window open] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns>
        /// <c>true</c> if [is window open] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
    }
}