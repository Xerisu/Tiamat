﻿using InitiativeBot.RNG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Rolling.Modifiers
{
    /// <summary>
    /// Adds number to a roll.
    /// </summary>
    public class AddConstantRollModifier : IRollModifier
    {
        private readonly IRoll _roll;
        private readonly int _constant;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="roll">Base roll.</param>
        /// <param name="constant">Number added to a roll.</param>
        public AddConstantRollModifier(IRoll roll, int constant)
        {
            _roll = roll;
            _constant = constant;
        }


        /// <inheritdoc/>
        public int RollDice(IRNG rng)
        {
            return _roll.RollDice(rng) + _constant;
        }

        public override string ToString()
        {
            return _roll.ToString() + _constant.ToString("+0;-#");
        }

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                AddConstantRollModifier r => r._constant == _constant && r._roll.Equals(_roll),
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return _constant ^ _roll.GetHashCode();
        }
    }
}
