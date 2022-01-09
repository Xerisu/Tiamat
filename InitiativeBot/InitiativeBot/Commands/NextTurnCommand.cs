using InitiativeBot.InitiativeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Commands
{
    /// <summary>
    /// Next turn
    /// </summary>
    public class NextTurnCommand : ICommand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NextTurnCommand()
        {

        }

        /// <inheritdoc/>
        public void Execute(IInitiativeList initiativeList)
        {
            initiativeList.NextTurn();
        }

        public override string ToString()
        {
            return "Switched to the next turn";
        }
    }
}
