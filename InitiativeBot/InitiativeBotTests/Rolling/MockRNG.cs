using InitiativeBot.RNG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
            return values[(n-1)%numberOfValues]%dice + 1;
        }
    }

    public class MockRNGTests
    {
        [Fact]
        public void RollDice_Should_ReturnProperInteger()
        {
            const int numberOfValues = 30;
            const int dice = 101;

            int[] expectedValues = new int[numberOfValues] { 41, 101, 46, 85, 67, 25, 45, 26, 79, 49, 56, 68, 38, 81, 51, 39, 69, 72, 95, 28, 41, 101, 46, 85, 67, 25, 45, 26, 79, 49 };

            var rng = new MockRNG();
            for(int i = 0; i < 30; i++)
            {
                int roll = rng.RollDice(dice);
                Assert.Equal(expectedValues[i], roll);
            }
        }

        [Theory]
        [InlineData(20, 1, 1)]
        [InlineData(20, 2, 1)]
        [InlineData(12, 3, 10)]
        [InlineData(8, 5, 3)]
        public void GetNthRollShould_ReturnProperValue(int dice, int n, int expectedValue)
        {
            var rng = new MockRNG();
            int result = rng.GetNthRoll(dice, n);
            Assert.Equal(expectedValue, result);
        }
    }
}
