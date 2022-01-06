using InitiativeBot.InitiativeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Commands
{
    /// <summary>
    /// Player is removed from the list
    /// </summary>
    public class KillCommand : ICommand
    {
        private readonly string _playerName;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="playerName">Name of the player to kill</param>
        public KillCommand(string playerName)
        {
            _playerName = playerName;
        }

        /// <inheritdoc/>
        public void Execute(IInitiativeList initiativeList)
        {
            Console.WriteLine($"Kill command executed. Killed player: {_playerName}.");
        }
    }
}
