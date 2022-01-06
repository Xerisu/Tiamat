using InitiativeBot.InitiativeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Commands
{
    /// <summary>
    /// Commands acting on a initiative list
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="initiativeList">Initiaive list that the command will act upon</param>
        void Execute(IInitiativeList initiativeList);
    }
}
