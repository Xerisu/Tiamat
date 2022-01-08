using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.RNG
{
    /// <summary>
    /// Provides random numbers
    /// </summary>
    public interface IRNG
    {
        /// <summary>
        /// Rolls a dice.
        /// </summary>
        /// <param name="dice">Number of sides on a die</param>
        /// <returns>Result of a roll</returns>
        /// <exception cref="ArgumentOutOfRangeException">If argument is 0 or less.</exception>
        int RollDice(int dice);
    }
}
