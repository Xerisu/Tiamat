using InitiativeBot.Commands;
using InitiativeBot.Rolling;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.InitiativeList
{
    /// <summary>
    /// Implementation of initiative list
    /// </summary>
    public class InitiativeList : IInitiativeList
    {
        private readonly List<Player.Player> _players = new();

        /// <inheritdoc/>
        public IReadOnlyList<Player.Player> Players => _players;

        public void AddPlayer(string name, IRoll roll)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ClearList()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void ExecuteCommand(ICommand command)
        {
            command.Execute(this);
        }

        /// <inheritdoc/>
        public void NextTurn()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void RemovePlayer(string name)
        {
            throw new NotImplementedException();
        }
    }
}
