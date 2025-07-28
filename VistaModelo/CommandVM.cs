using System;
using System.Windows.Input;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class CommandVM : ICommand
    {
        private readonly Action<object> _executeAction;//accion que puede ejecutar el comando o no
        private readonly Predicate<object> _canExecuteAction;

        public CommandVM(Action<object> executeAction)//sin solicitud
        {
            _executeAction = executeAction;
            _canExecuteAction = null;
        }
        public CommandVM(Action<object> executeAction, Predicate<object> canExecuteAction)//con solicitud
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        public event EventHandler CanExecuteChanged//define el administrador de comando para detectar que la ejecucion cambia.
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteAction == null ? true : _canExecuteAction(parameter);// si es nulo retornar falso y si tiene una respuesta define true para llamar al metodo delegado
        }

        public void Execute(object? parameter)
        {
            _executeAction(parameter);
        }
    }
}
