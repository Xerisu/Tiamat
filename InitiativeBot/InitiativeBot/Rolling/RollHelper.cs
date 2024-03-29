﻿using InitiativeBot.Parser.JoinModifier;
using InitiativeBot.Rolling.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Rolling
{
    /// <summary>
    /// Utility class for rolls
    /// </summary>
    public static class RollHelper
    {
        /// <summary>
        /// Creates a roll from base die and list of modifiers.
        /// </summary>
        /// <param name="baseDie">Base dice to use.</param>
        /// <param name="modifiers">List of modifiers.</param>
        /// <returns>A roll.</returns>
        /// <exception cref="ArgumentException">Roll has more than one (dis)advantage modifier.</exception>
        public static IRoll BuildRollFromJoinModifiers(int baseDie, IJoinModifier[] modifiers)
        {
            IRoll roll = new Roll(baseDie);

            var advantageRolls = modifiers.Where(modifier => modifier is AdvantageModifier || modifier is DisadvantageModifier);
            if (advantageRolls.Count() > 1)
                throw new ArgumentException("Roll has more than one (dis)advantage modifier.");

            var advantageModifier = advantageRolls.FirstOrDefault();
            roll = advantageModifier switch
            {
                AdvantageModifier => new AdvantageRollModifier(roll),
                DisadvantageModifier => new DisadvantageRollModifier(roll),
                _ => roll
            };

            var constatModifiers = modifiers.Where(modifier => modifier is ConstantModifier);
            foreach(var modifier in constatModifiers)
            {
                if (modifier is ConstantModifier cm)
                    roll = new AddConstantRollModifier(roll, cm.Constant);
            }

            var diceModifiers = modifiers.Where(modifier => modifier is DiceWithMultiplierModifier);
            foreach(var modifier in diceModifiers)
            {
                if (modifier is DiceWithMultiplierModifier dm)
                {
                    IRoll addedRoll = new Roll(dm.Dice);
                    addedRoll = new MultiplyingRollModifier(addedRoll, dm.Multiplier);
                    roll = new AddDiceRollModifier(roll, addedRoll);
                }
            }

            return roll;
        }
    }
}
