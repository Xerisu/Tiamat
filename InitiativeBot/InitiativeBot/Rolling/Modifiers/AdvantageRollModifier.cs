using InitiativeBot.RNG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Rolling.Modifiers
{
    /// <summary>
    /// Rolling 2 dice and getting higher result.
    /// </summary>
    public class AdvantageRollModifier : IRollModifier
    {
        private readonly IRoll _roll;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="roll">Base roll.</param>
        public AdvantageRollModifier(IRoll roll)
        {
            _roll = roll;
        }

        /// <inheritdoc/>
        public int RollDice(IRNG rng)
        {
            int roll1 = _roll.RollDice(rng);
            int roll2 = _roll.RollDice(rng);
            return Math.Max(roll1, roll2);

        }
        public override string ToString()
        {
            return $"adv({_roll})";
        }
    }
}
