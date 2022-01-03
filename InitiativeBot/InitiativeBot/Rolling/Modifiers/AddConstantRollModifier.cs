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
        private IRoll _roll;
        private int _constant;

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
        public int RollDice()
        {
            return _roll.RollDice() + _constant;
        }

        public override string ToString()
        {
            return _roll.ToString() + _constant.ToString("+0;-#");
        }
    }
}
