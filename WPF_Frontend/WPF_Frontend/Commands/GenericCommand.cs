using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_Frontend.Commands
{
    public class GenericCommand : ICommand
    {
        private Action<object> _action;

        public GenericCommand(Action<object> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action), "Cannot be null!");
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _action(parameter);
        }
    }
}
