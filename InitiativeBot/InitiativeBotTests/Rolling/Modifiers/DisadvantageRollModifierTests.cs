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
    public class DisadvantageRollModifierTests
    {
        private const int _testSeed = 15;

        [Theory]
        [InlineData(24)]
        [InlineData(314)]
        public void DisadvantageDiceRollModifierTests_Should_ReturnExpectedValuesForVariableDices(int dice)
        {
            int[] expectList = RNG.GetStreamOfRandomNumbersFromSeed(_testSeed, dice).Take(2).ToArray();
            int expect = Math.Min(expectList[0], expectList[1]);
            RNG.SetSeed(_testSeed);
            IRoll rolling = new Roll(dice);
            rolling = new DisadvantageRollModifier(rolling);
            int roll = rolling.RollDice();
            Assert.Equal(expect, roll);
        }
    }
}
