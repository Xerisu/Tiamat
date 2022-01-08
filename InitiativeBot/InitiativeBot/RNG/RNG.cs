using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBot.RNG
{
    /// <summary>
    /// Implements rolling dice
    /// </summary>
    public class RNG : IRNG
    {
        private readonly Random random;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="seed">Seed, null for random.</param>
        public RNG(int? seed = null)
        {
            random = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        /// <inheritdoc/>
        public int RollDice(int dice)
        {
            if (dice < 1)
                throw new ArgumentOutOfRangeException(nameof(dice), dice, $"Dice has to be greater than 0. Provided {dice}");
            return random.Next(dice) + 1;
        }
    }
}
