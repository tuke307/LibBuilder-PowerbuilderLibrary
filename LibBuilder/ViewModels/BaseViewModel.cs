using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibBuilder.ViewModels
{
    /// <summary>
    /// BaseViewModel
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The property backing fields
        /// </summary>
        private Dictionary<string, object> propertyBackingFields = new Dictionary<string, object>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the calling property.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>The value of the backing field of the property.</returns>
        protected T Get<T>([CallerMemberName] string name = null)
        {
            object value = null;
            if (propertyBackingFields.TryGetValue(name, out value))
            {
                return value == null ? default(T) : (T)value;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Sets the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the calling property.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        protected void Set<T>(T value, [CallerMemberName] string name = null)
        {
            if (Equals(value, Get<T>(name)))
            {
                return;
            }
            else
            {
                propertyBackingFields[name] = value;
                OnPropertyChanged(name);
            }
        }
    }
}