using InitiativeBot.Commands;
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
        private List<Player.Player> _players = new();

        /// <inheritdoc/>
        public IReadOnlyList<Player.Player> Players => _players;

        /// <inheritdoc/>
        public void ExecuteCommand(ICommand command)
        {
            command.Execute(this);
        }
    }
}
