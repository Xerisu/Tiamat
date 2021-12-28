using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.Rolling
{
    /// <summary>
    /// Rolling dice utility.
    /// </summary>
    public static class RNG
    {
        private static Random random = new();

        /// <summary>
        /// Sets a seed for rolling.
        /// </summary>
        /// <param name="seed">Seed, null for random.</param>
        public static void SetSeed(int? seed)
        {
            random = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        /// <summary>
        /// Rolls a dice.
        /// </summary>
        /// <param name="dice">Number of sides on a die, should be >= 1.</param>
        /// <returns>Result of a roll</returns>
        /// <exception cref="ArgumentOutOfRangeException">If argument is 0 or less.</exception>
        public static int RollDice(int dice)
        {
            if (dice < 1)
                throw new ArgumentOutOfRangeException($"Dice has to be greater than 0. Provided {dice}");
            return random.Next(dice) + 1;
        }
    }
}
