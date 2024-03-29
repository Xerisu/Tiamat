﻿using InitiativeBot.RNG;
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
        public int RollDice(IRNG rng)
        {
            return rng.RollDice(_diceType);
        }

        public override string ToString()
        {
            return $"d{_diceType}";
        }

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                Roll r => r._diceType == _diceType,
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return _diceType ^ base.GetHashCode();
        }
    }
}
