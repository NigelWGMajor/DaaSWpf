using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DaaSWpf
{
    public class CommandClass : ICommand
    {
        /// <summary>
        /// Generate an ICommand suitable for WPF binding.
        /// - XAML: <Button Command="{Binding MyCommand}" Content="ButtonTitle" />
        /// - VM: public ICommand MyCommand {  get { return myCommand; }}
        ///       ICommand myCommand = new CommandClass
        ///       { x => myPredicate(x),
        ///         x => whatToDoWith(x)
        ///       }
        /// </summary>
        /// <param name="canExecute">a lambda or method returning bool (Predicate)</param>
        /// <param name="execute">a lambda or method returning void (Action)</param>
        public CommandClass(Predicate<object> canExecute, Action<object> execute)
        {
            CanExecuteDelegate = canExecute;
            ExecuteDelegate = execute;
        }
        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }
        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate(parameter);
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
                ExecuteDelegate(parameter);
        }
    }
}
