using InitiativeBot.InitiativeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Commands
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
        public void Execute(IInitiativeList initiativeList)
        {
            initiativeList.ClearList();
        }

        public override string ToString()
        {
            return "List cleared";
        }
    }
}
