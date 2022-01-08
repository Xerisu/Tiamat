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
    public class AdvantageRollModifierTests
    {
        [Theory]
        [InlineData(20)]
        [InlineData(12)]
        public void AdvantageDiceRollModifierTests_Should_ReturnExpectedValuesForVariableDices(int dice)
        {
            MockRNG rng = new();

            int expect = Math.Max(rng.GetNthRoll(dice, 1), rng.GetNthRoll(dice, 2));

            IRoll rolling = new Roll(dice);
            rolling = new AdvantageRollModifier(rolling);
            int roll = rolling.RollDice(rng);

            Assert.Equal(expect, roll);
        }
    }
}
