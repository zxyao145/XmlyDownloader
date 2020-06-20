using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XmlyDownloader.Commands
{
    public interface IAsyncCommand:ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
