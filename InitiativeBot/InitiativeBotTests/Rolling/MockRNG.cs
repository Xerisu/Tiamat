using InitiativeBot.RNG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeBotTests.Rolling
{
    /// <summary>
    /// Mocked RNG for tests that allows to get any roll beforehand
    /// </summary>
    internal class MockRNG : IRNG
    {
        private const int numberOfValues = 20;
        private int actualValue = 0;
        private readonly int[] values = new int[numberOfValues] { 40, 100, 45, 84, 66, 24, 44, 25, 78, 48, 55, 67, 37, 80, 50, 38, 68, 71, 94, 27 };

        /// <inheritdoc/>
        public int RollDice(int dice)
        {
            int returnedValue = values[actualValue] % dice + 1;
            actualValue++;
            while (actualValue >= numberOfValues)
                actualValue -= numberOfValues;
            return returnedValue;
        }

        /// <summary>
        /// Gets nth roll in the sequence
        /// </summary>
        /// <param name="dice">dice</param>
        /// <param name="n">n >= 1</param>
        /// <returns></returns>
        public int GetNthRoll(int dice, int n)
        {
            return values[(n+1)%numberOfValues]%dice + 1;
        }
    }
}
