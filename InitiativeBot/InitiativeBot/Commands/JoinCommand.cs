using InitiativeBot.InitiativeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Commands
{
    /// <summary>
    /// Player joining the list
    /// </summary>
    public class JoinCommand : ICommand
    {
        private readonly string _playerName;
        private readonly string[] _modifiers;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="playerName">Name of the new player, if already exists - reroll initiative</param>
        /// <param name="modifiers">List of modifiers</param>
        public JoinCommand(string playerName, string[] modifiers)
        {
            _playerName = playerName;
            _modifiers = modifiers;
        }

        /// <inheritdoc/>
        public void Execute(IInitiativeList initiativeList)
        {
            Console.WriteLine("Join command executed.");
        }
    }
}
