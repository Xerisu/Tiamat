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
        [Theory]
        [InlineData(2, 4)]
        [InlineData(-3, 6)]
        public void MultiplyingRollModifierTests_Should_ReturnExpectedValuesForVariableDicesAndMultipliers(int multiplier, int dice)
        {
            MockRNG rng = new();

            int expect = 0;
            for(int i = 0; i < Math.Abs(multiplier); i++)
            {
                expect += rng.GetNthRoll(dice, i + 1);
            }
            expect *= Math.Sign(multiplier);

            IRoll rolling = new Roll(dice);
            rolling = new MultiplyingRollModifier(rolling, multiplier);
            int roll = rolling.RollDice(rng);

            Assert.Equal(expect, roll);
        }
    }
}
