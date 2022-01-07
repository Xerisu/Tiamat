using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.InitiativeList.Player
{
    /// <summary>
    /// Status of the player
    /// </summary>
    public enum PlayerState
    {
        active,
        unactive,
        unconscious,
        dead
    }

    /// <summary>
    /// Player on the initiative list
    /// </summary>
    public struct Player
    {
        /// <summary>
        /// Name of the player
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// State of the player
        /// </summary>
        public PlayerState State { get; set; }

        /// <summary>
        /// Initiative of the player
        /// </summary>
        public int Initiative { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the player</param>
        /// <param name="initiative">State of the player</param>
        /// <param name="playerState">Initiative of the player</param>
        public Player(string name, int initiative, PlayerState playerState = PlayerState.unactive)
        {
            Name = name;
            State = playerState;
            Initiative = initiative;
        }
    }
}
