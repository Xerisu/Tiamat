using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniciativeBot.Commands
{
    /// <summary>
    /// Commands acting on a initiative list
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Execute command
        /// </summary>
        void Execute();
    }
}
