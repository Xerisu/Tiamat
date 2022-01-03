using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Rolling
{
    /// <summary>
    /// Rolls a die.
    /// </summary>
    public class Roll : IRoll
    {
        private int _diceType;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dice">Number of sides.</param>
        public Roll(int dice)
        {
            _diceType = dice;
        }

        /// <inheritdoc/> 
        public int RollDice()
        {
            return RNG.RollDice(_diceType);
        }

        public override string ToString()
        {
            return $"d{_diceType}";
        }
    }
}
