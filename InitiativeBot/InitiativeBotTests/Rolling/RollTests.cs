using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitiativeBot.RNG;
using InitiativeBot.Rolling;
using Moq;
using Xunit;

namespace InitiativeBotTests.Rolling
{
    public class RollTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(12)]
        public void RollDice_Should_ReturnExpectedValuesForVariableDice(int dice)
        {
            MockRNG rng = new();

            int expect = rng.GetNthRoll(dice, 1);

            IRoll rolling = new Roll(dice);
            int roll = rolling.RollDice(rng);
            Assert.Equal(expect, roll);
        }
    }
}
