﻿using InitiativeBot.RNG;
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
        private readonly IRoll _roll;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="roll">Base roll.</param>
        public DisadvantageRollModifier(IRoll roll)
        {
            _roll = roll;
        }

        /// <inheritdoc/>
        public int RollDice(IRNG rng)
        {
            int roll1 = _roll.RollDice(rng);
            int roll2 = _roll.RollDice(rng);
            return Math.Min(roll1, roll2);

        }
        public override string ToString()
        {
            return $"dis({_roll})";
        }

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                DisadvantageRollModifier r => r._roll.Equals(_roll),
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return _roll.GetHashCode() ^ 0x0ab0cfab;
        }
    }

}
