using InitiativeBot.InitiativeList;
using InitiativeBot.Parser.JoinModifier;
using InitiativeBot.Rolling;
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
        private readonly IRoll _roll;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="playerName">Name of the new player, if already exists - reroll initiative</param>
        /// <param name="modifiers">List of modifiers</param>
        public JoinCommand(string playerName, int baseDie, IJoinModifier[] modifiers)
        {
            _playerName = playerName;
            _roll = RollHelper.BuildRollFromJoinModifiers(baseDie, modifiers);
        }

        /// <inheritdoc/>
        public void Execute(IInitiativeList initiativeList)
        {
            Console.WriteLine($"Join command executed. Player: {_playerName}. Roll: {_roll}");
        }
    }
}
