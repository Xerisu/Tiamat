using InitiativeBot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.InitiativeList
{
    /// <summary>
    /// Initiative List
    /// </summary>
    public interface IInitiativeList
    {
        /// <summary>
        /// List of players
        /// </summary>
        IReadOnlyList<Player.Player> Players { get; }

        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="command">Command to execute</param>
        void ExecuteCommand(ICommand command);
    }
}
