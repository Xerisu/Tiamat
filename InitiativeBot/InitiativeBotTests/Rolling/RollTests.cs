using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InitiativeBot.Rolling;

using Xunit;

namespace InitiativeBotTests.Rolling
{
    public class RollTests
    {
        private const int _testSeed = 15;

        [Theory]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(12)]
        public void RollDice_Should_ReturnExpectedValuesForVariableDice(int dice)
        {
            int expect = RNG.GetStreamOfRandomNumbersFromSeed(_testSeed, dice).Take(1).ToArray()[0];
            RNG.SetSeed(_testSeed);
            IRoll rolling = new Roll(dice);
            int roll = rolling.RollDice();
            Assert.Equal(expect, roll);
        }
    }
}
