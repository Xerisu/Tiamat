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
        private const int _testSeed = 15;

        [Theory]
        [InlineData(20, 10)]
        [InlineData(20, 5)]
        [InlineData(12, -4)]
        public void AddConstantRollModifier_Should_ReturnExpectedValuesForVariableConst(int dice, int constant)
        {
            int expect = RNG.GetStreamOfRandomNumbersFromSeed(_testSeed, dice).Take(1).ToArray()[0];
            RNG.SetSeed(_testSeed);
            IRoll rolling = new Roll(dice);
            rolling = new AddConstantRollModifier(rolling, constant);
            int roll = rolling.RollDice();
            Assert.Equal(expect + constant, roll);
        }
    }
}
