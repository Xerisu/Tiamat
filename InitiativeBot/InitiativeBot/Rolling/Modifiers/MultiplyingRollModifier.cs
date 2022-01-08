using InitiativeBot.RNG;
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
        public int RollDice(IRNG rng)
        {
            int betterMultiplier = _multiplier;
            bool multiplierNegative = false;
            if (betterMultiplier < 0)
            {
                betterMultiplier = betterMultiplier * -1;
                multiplierNegative = true;
            }
            int roll = 0;
            for (int i = 0; i < betterMultiplier; i++) roll += _roll.RollDice(rng);
            return multiplierNegative ? -roll : roll;
        }

        public override string ToString()
        {
            return _multiplier.ToString() + _roll.ToString();
        }

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                MultiplyingRollModifier r => r._multiplier == _multiplier && r._roll.Equals(_roll),
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return _multiplier ^ _roll.GetHashCode();
        }
    }
   
}
