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
    public class RemoveCommand : ICommand
    {
        private readonly string _playerName;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="playerName">Name of the player to kill</param>
        public RemoveCommand(string playerName)
        {
            _playerName = playerName;
        }

        /// <inheritdoc/>
        public void Execute(IInitiativeList initiativeList)
        {
            initiativeList.RemovePlayer(_playerName);
        }

        public override string ToString()
        {
            return $"Player {_playerName} removed from the list";
        }
    }
}
