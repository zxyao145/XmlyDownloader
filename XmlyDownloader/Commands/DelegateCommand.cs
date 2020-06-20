using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XmlyDownloader.Commands
{
    public class DelegateCommand : IAsyncCommand
    {

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && (CanExecuteFunc?.Invoke(parameter) ?? true);
        }

        public Func<object, bool> CanExecuteFunc { get; set; }


        public async void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                ExecuteAction?.Invoke(parameter);
                await ExecuteAsync(parameter);
            }
        }
        public Action<object> ExecuteAction { get; set; }


        private bool _isExecuting;

        public async Task ExecuteAsync(object parameter)
        {
            if (CanExecute(parameter))
            {
                if(ExecuteActionAsync != null)
                {
                    try
                    {
                        _isExecuting = true;
                        await ExecuteActionAsync(parameter);
                    }
                    finally
                    {
                        _isExecuting = false;
                    }
                }
               
            }

            RaiseCanExecuteChanged();
        }
        public Func<object,Task> ExecuteActionAsync { get; set; }

    }
}
