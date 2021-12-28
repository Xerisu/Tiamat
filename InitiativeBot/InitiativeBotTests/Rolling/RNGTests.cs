using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InitiativeBot.Rolling;

using Xunit;

namespace InitiativeBotTests.Rolling
{
    public class RNGTests
    {
        [Fact]
        public void RollDice_Should_ReturnRandomInteger()
        {
            int[] expectedValues = new int[] { 12, 5, 6, 16, 20, 14, 13, 20, 17, 17, 7, 4, 19, 6, 10, 8, 4, 6, 2, 3 };
            RNG.SetSeed(15);
            for (int i = 0; i < 20; i++)
            {
                int roll = RNG.RollDice(20);
                Assert.Equal(expectedValues[i], roll);
            }
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-15)]
        public void RollDice_Should_ThrowException_When_DiceIsLowerThan1(int dice)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => RNG.RollDice(dice));
        }
    }
}
