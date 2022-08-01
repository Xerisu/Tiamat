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
    /// Initiative List
    /// </summary>
    public interface IInitiativeList
    {
        /// <summary>
        /// List of players
        /// </summary>
        IReadOnlyList<Player.Player> Players { get; }

        /// <summary>
        /// Current round.
        /// </summary>
        int Round { get; }
        /// <summary>
        /// Current player taking turn.
        /// </summary>
        int ActivePlayerIndex { get; }

        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="command">Command to execute</param>
        void ExecuteCommand(ICommand command);

        /// <summary>
        /// Adding player to initiative list.
        /// </summary>
        /// <param name="name">Name of a player.</param>
        /// <param name="roll">Initiative roll.</param>
        void AddPlayer(string name, IRoll roll);

        /// <summary>
        /// Going to the next turn.
        /// </summary>
        void NextTurn();

        /// <summary>
        /// Removing player from the initiative list.
        /// </summary>
        /// <param name="name">Name of a player.</param>
        void RemovePlayer(string name);

        /// <summary>
        /// Clearing initiative list.
        /// </summary>
        void ClearList();
    }
}
