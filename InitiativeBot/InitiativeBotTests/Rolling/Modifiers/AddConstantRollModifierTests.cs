using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InitiativeBot.Rolling;
using InitiativeBot.Rolling.Modifiers;

using Xunit;

namespace InitiativeBotTests.Rolling.Modifiers
{
    public class AddConstantRollModifierTests
    {
        [Theory]
        [InlineData(20, 10)]
        [InlineData(20, 5)]
        [InlineData(12, -4)]
        public void AddConstantRollModifier_Should_ReturnExpectedValuesForVariableConst(int dice, int constant)
        {
            MockRNG rng = new();

            int expect = rng.GetNthRoll(dice, 1);

            IRoll rolling = new Roll(dice);
            rolling = new AddConstantRollModifier(rolling, constant);
            int roll = rolling.RollDice(rng);

            Assert.Equal(expect + constant, roll);
        }
    }
}
