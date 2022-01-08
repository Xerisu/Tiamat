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
        private const int _testSeed = 15;

        [Theory]
        [InlineData(20, 4)]
        [InlineData(20, 5)]
        [InlineData(20, 45)]
        public void AddDiceRollModifierTests_Should_ReturnExpectedValuesForVariableDices(int dice1, int dice2)
        {
            int expect1 = RNG.GetStreamOfRandomNumbersFromSeed(_testSeed, dice1).Take(1).ToArray()[0];
            int expect2 = RNG.GetStreamOfRandomNumbersFromSeed(_testSeed, dice2).Take(2).ToArray()[1];
            RNG.SetSeed(_testSeed);
            IRoll rolling1 = new Roll(dice1);
            IRoll rolling2 = new Roll(dice2);
            IRoll rolling = new AddDiceRollModifier(rolling1, rolling2);
            int roll = rolling.RollDice();
            Assert.Equal(expect1 + expect2, roll);
        }
    }
}
