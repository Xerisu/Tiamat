using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Rolling.Modifiers
{
    /// <summary>
    /// Rolling 2 dice and getting lower result.
    /// </summary>
    public class DisadvantageRollModifier : IRollModifier
    {
        private IRoll _roll;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="roll">Base roll.</param>
        public DisadvantageRollModifier(IRoll roll)
        {
            _roll = roll;
        }

        /// <inheritdoc/>
        public int RollDice()
        {
            int roll1 = _roll.RollDice();
            int roll2 = _roll.RollDice();
            return Math.Min(roll1, roll2);

        }
        public override string ToString()
        {
            return $"dis({_roll})";
        }
    }

}
