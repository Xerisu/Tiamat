using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InitiativeBot.Rolling.Modifiers;
using InitiativeBot.Rolling;
using Xunit;

namespace InitiativeBotTests.Rolling.Modifiers
{
    public class MultiplyingRollModifierTests
    {
        private const int _testSeed = 15;

        [Theory]
        [InlineData(2, 4)]
        [InlineData(-3, 6)]
        public void MultiplyingRollModifierTests_Should_ReturnExpectedValuesForVariableDicesAndMultipliers(int multiplier, int dice)
        {
            int[] expectList = RNG.GetStreamOfRandomNumbersFromSeed(_testSeed, dice).Take(Math.Abs(multiplier)).ToArray();
            int expect = expectList.Sum() * Math.Sign(multiplier);
            RNG.SetSeed(_testSeed);
            IRoll rolling = new Roll(dice);
            rolling = new MultiplyingRollModifier(rolling, multiplier);
            int roll = rolling.RollDice();
            Assert.Equal(expect, roll);
        }
    }
}
