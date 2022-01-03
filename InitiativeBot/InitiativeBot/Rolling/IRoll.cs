using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Rolling
{
    /// <summary>
    /// Interface for rolling a die.
    /// </summary>
    public interface IRoll
    {
        /// <summary>
        /// Rolls a die.
        /// </summary>
        /// <returns>Roll result.</returns>
        int RollDice();
    }
}
