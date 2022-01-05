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
        private const int _testSeed = 15;
        private const int _testDice = 20;
        private const int _numerOfTestRolls = 20;
        private static readonly int[] _twentyExpectedValuesForSeed15Dice20 = new int[] { 12, 5, 6, 16, 20, 14, 13, 20, 17, 17, 7, 4, 19, 6, 10, 8, 4, 6, 2, 3 };


        [Fact]
        public void RollDice_Should_ReturnRandomInteger()
        {
            RNG.SetSeed(_testSeed);
            for (int i = 0; i < _numerOfTestRolls; i++)
            {
                int roll = RNG.RollDice(_testDice);
                Assert.Equal(_twentyExpectedValuesForSeed15Dice20[i], roll);
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

        [Fact]
        public void GetStreamOfRandomNumbersFromSeed_Should_ReturnExpectedValuesForSeed()
        {
            var diceRolls = RNG.GetStreamOfRandomNumbersFromSeed(_testSeed, _testDice);
            var rolls = diceRolls.Take(_numerOfTestRolls).ToArray();
            for(int i = 0; i < _numerOfTestRolls; i++)
            {
                Assert.Equal(_twentyExpectedValuesForSeed15Dice20[i], rolls[i]);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-15)]
        public void GetStreamOfRandomNumbersFromSeed_Should_ThrowException_When_DiceIsLowerThan1(int dice)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var diceRolls = RNG.GetStreamOfRandomNumbersFromSeed(0, dice);
                var roll = diceRolls.Take(1).ToArray()[0];
            });
        }
    }
}
