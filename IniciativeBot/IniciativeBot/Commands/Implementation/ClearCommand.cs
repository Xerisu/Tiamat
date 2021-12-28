using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniciativeBot.Commands
{
    /// <summary>
    /// List is cleared
    /// </summary>
    public class ClearCommand : ICommand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ClearCommand()
        {

        }

        /// <inheritdoc/>
        public void Execute()
        {
            Console.WriteLine("Clear command executed.");
        }
    }
}
