using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibBuilder.Business
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }

    public abstract class AsyncCommandBase : IAsyncCommand
    {
        public abstract bool CanExecute(object parameter);

        public abstract Task ExecuteAsync(object parameter);

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class AsyncCommand : AsyncCommandBase
    {
        private readonly Func<Task> _command;

        public AsyncCommand(Func<Task> command)
        {
            _command = command;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override Task ExecuteAsync(object parameter)
        {
            return _command();
        }
    }

    /// <summary>
    /// Command handler
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class ActionCommand : ICommand
    {
        private readonly Func<object, bool> canExecuteHandler;
        private readonly Action<object> executeHandler;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should
        /// execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <exception cref="ArgumentNullException">Execute cannot be null</exception>
        public ActionCommand(Action<object> execute)
        {
            executeHandler = execute ?? throw new ArgumentNullException($"{nameof(execute)} cannot be null.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public ActionCommand(Action<object> execute, Func<object, bool> canExecute)
            : this(execute)
        {
            executeHandler = execute;
            canExecuteHandler = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its
        /// current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to <see langword="null" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if this command can be executed; otherwise, <see
        /// langword="false" />.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            if (canExecuteHandler == null)
            {
                return true;
            }

            return canExecuteHandler(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to <see langword="null" />.
        /// </param>
        public void Execute(object parameter)
        {
            executeHandler(parameter);
        }
    }
}