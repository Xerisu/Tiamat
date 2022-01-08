using InitiativeBot.Rolling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Parser.JoinModifier
{
    /// <summary>
    /// Token for modifier
    /// </summary>
    public interface IJoinModifier
    {
    }

    /// <summary>
    /// Modifier adding advantage to the roll
    /// </summary>
    public class AdvantageModifier : IJoinModifier
    {
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            return obj is AdvantageModifier;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Modifier adding disadvantage to the roll
    /// </summary>
    public class DisadvantageModifier : IJoinModifier
    {
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            return obj is DisadvantageModifier;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Modifier adding constant to the roll
    /// </summary>
    public class ConstantModifier : IJoinModifier
    {
        /// <summary>
        /// Constant
        /// </summary>
        public int Constant { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="constant">Constant to add</param>
        public ConstantModifier(int constant)
        {
            Constant = constant;
        }

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                ConstantModifier cm => cm.Constant == Constant,
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return Constant ^ base.GetHashCode();
        }
    }

    /// <summary>
    /// Modifier adding dice to the roll (with possible multipliers)
    /// </summary>
    public class DiceWithMultiplierModifier : IJoinModifier
    {
        /// <summary>
        /// Dice to roll
        /// </summary>
        public int Dice { get; }
        /// <summary>
        /// Multiplier to use
        /// </summary>
        public int Modifier { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="multiplier">Multiplier to use</param>
        /// <param name="dice">Dice to roll</param>
        public DiceWithMultiplierModifier(int multiplier, int dice)
        {
            Modifier = multiplier;
            Dice = dice;
        }

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                DiceWithMultiplierModifier dwmm => dwmm.Dice == Dice && dwmm.Modifier == Modifier,
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return (Dice * Modifier) ^ base.GetHashCode();
        }
    }
    
}
