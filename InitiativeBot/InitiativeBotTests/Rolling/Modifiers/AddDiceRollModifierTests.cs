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
    public class AddDiceRollModifierTests
    {
        [Theory]
        [InlineData(20, 4)]
        [InlineData(20, 5)]
        [InlineData(20, 45)]
        public void AddDiceRollModifierTests_Should_ReturnExpectedValuesForVariableDices(int dice1, int dice2)
        {
            MockRNG rng = new();

            int expect = rng.GetNthRoll(dice1, 1) + rng.GetNthRoll(dice2, 2);

            IRoll rolling1 = new Roll(dice1);
            IRoll rolling2 = new Roll(dice2);
            IRoll rolling = new AddDiceRollModifier(rolling1, rolling2);

            int roll = rolling.RollDice(rng);

            Assert.Equal(expect, roll);
        }
    }
}
