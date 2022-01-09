using InitiativeBot.Parser.JoinModifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InitiativeBotTests.Parser
{
    public class ParserTests
    {

        // Data for join modifiers parser tests with valid data.
        // First object is string to parse, second is array with parssed modifiers
        public static IEnumerable<object[]> JoinModifiersValidData => new List<object[]>
        {
            new object[] {"adv +2", new IJoinModifier[] {
                new AdvantageModifier(),
                new ConstantModifier(2)
            } },
            new object[] {"2d8+3adv-2", new IJoinModifier[]{
                new DiceWithMultiplierModifier(2, 8),
                new ConstantModifier(3),
                new AdvantageModifier(),
                new ConstantModifier(-2)
            } },
            new object[] {"d5-2dis-2d9 +3 dis", new IJoinModifier[]
            {
                new DiceWithMultiplierModifier(1, 5),
                new ConstantModifier(-2),
                new DisadvantageModifier(),
                new DiceWithMultiplierModifier(-2, 9),
                new ConstantModifier(3),
                new DisadvantageModifier()
            } },
            new object[] {"-2+2-2d2+2d2advdis", new IJoinModifier[] {
                new ConstantModifier(-2),
                new ConstantModifier(2),
                new DiceWithMultiplierModifier(-2, 2),
                new DiceWithMultiplierModifier(2, 2),
                new AdvantageModifier(),
                new DisadvantageModifier()
            } },
            new object[] {"", Array.Empty<IJoinModifier>() },
        };

        [Theory]
        [MemberData(nameof(JoinModifiersValidData))]
        public void ParseJoinModifiersString_Should_ReturnProperModifiersInOrder(string modifierString, IJoinModifier[] expectedModifiers)
        {
            var parseResult = InitiativeBot.Parser.Parser.ParseJoinModifiersString(modifierString);
            Assert.Equal(expectedModifiers.Length, parseResult.Length);

            for(int i = 0; i < expectedModifiers.Length; i++)
            {
                Assert.Equal(expectedModifiers[i], parseResult[i]);
            }
        }

        [Theory]
        [InlineData("asasadasd")]
        [InlineData("addis+2+2")]
        [InlineData("+2d8-2diadv")]
        [InlineData("--++")]
        [InlineData("-2--")]
        public void ParseJoinModifiersString_Should_ThrowArgumentException_When_InputIsInvalid(string modifierString)
        {
            Assert.Throws<ArgumentException>(() => InitiativeBot.Parser.Parser.ParseJoinModifiersString(modifierString));
        }
    }
}
