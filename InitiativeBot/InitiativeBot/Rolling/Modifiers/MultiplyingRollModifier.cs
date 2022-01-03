using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Rolling.Modifiers
{
    /// <summary>
    /// Multiplying a roll.
    /// </summary>
    public class MultiplyingRollModifier : IRollModifier
    {
        private readonly IRoll _roll;
        private readonly int _multiplier;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="roll">Base roll.</param>
        /// <param name="multiplier">Roll multiplier.</param>
        public MultiplyingRollModifier(IRoll roll, int multiplier)
        {
            _roll = roll;
            _multiplier = multiplier;
        }

        /// <inheritdoc/>
        public int RollDice()
        {
            return _multiplier * _roll.RollDice();
        }

        public override string ToString()
        {
            return _multiplier.ToString() + _roll.ToString();
        }
    }

   
}
