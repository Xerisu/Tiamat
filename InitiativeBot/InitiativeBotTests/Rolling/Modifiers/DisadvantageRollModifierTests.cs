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
        [Theory]
        [InlineData(24)]
        [InlineData(314)]
        public void DisadvantageDiceRollModifierTests_Should_ReturnExpectedValuesForVariableDices(int dice)
        {
            MockRNG rng = new();

            int expect = Math.Min(rng.GetNthRoll(dice, 1), rng.GetNthRoll(dice, 2));

            IRoll rolling = new Roll(dice);
            rolling = new DisadvantageRollModifier(rolling);
            int roll = rolling.RollDice(rng);

            Assert.Equal(expect, roll);
        }
    }
}
