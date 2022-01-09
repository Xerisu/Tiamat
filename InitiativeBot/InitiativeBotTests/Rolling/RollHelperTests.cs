using InitiativeBot.Parser.JoinModifier;
using InitiativeBot.Rolling;
using InitiativeBot.Rolling.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InitiativeBotTests.Rolling
{
    public class RollHelperTests
    {
        private const int _baseDie = 20;

        // Valid data with roll and modifiers that should create this roll
        // First object is list of modifiers, second is roll
        public static IEnumerable<object[]> BuildRollFromModifiersValidData => new List<object[]>
        {
            new object[] {
                new IJoinModifier[] {
                    new DisadvantageModifier(),
                    new ConstantModifier(2)
                },
                new AddConstantRollModifier(new DisadvantageRollModifier(new Roll(_baseDie)), 2)
            },
            new object[] {
                new IJoinModifier[] {
                    new DiceWithMultiplierModifier(2, 8),
                    new ConstantModifier(3),
                    new AdvantageModifier(),
                    new ConstantModifier(-2)
                }, 
                new AddDiceRollModifier(
                    new AddConstantRollModifier(
                        new AddConstantRollModifier(
                            new AdvantageRollModifier(new Roll(_baseDie)),
                            3),
                        -2),
                    new MultiplyingRollModifier(
                        new Roll(8),
                2)
                )
            },
            new object[]
            {
                new IJoinModifier[] {
                    new ConstantModifier(2),
                    new ConstantModifier(3),
                    new DiceWithMultiplierModifier(-3, 12),
                },
                new AddDiceRollModifier(
                    new AddConstantRollModifier(new AddConstantRollModifier(new Roll(_baseDie), 2), 3),
                    new MultiplyingRollModifier(new Roll(12), -3)
                )
            },
            new object[] { Array.Empty<IJoinModifier>(), new Roll(_baseDie) },
        };

        [Theory]
        [MemberData(nameof(BuildRollFromModifiersValidData))]
        public void BuildRollFromJoinModifiers_Should_BuildProperRoll(IJoinModifier[] modifiers, IRoll expectedRoll)
        {
            IRoll resultRoll = RollHelper.BuildRollFromJoinModifiers(_baseDie, modifiers);

            Assert.Equal(expectedRoll, resultRoll);
        }

        // Invalid data with modifiers that should throw
        // The only object is list of modifiers
        public static IEnumerable<object[]> BuildRollFromModifiersThrowData = new List<object[]>
        {
            new object[] { new IJoinModifier[]
            {
                new DiceWithMultiplierModifier(1, 5),
                new ConstantModifier(-2),
                new DisadvantageModifier(),
                new DiceWithMultiplierModifier(-2, 9),
                new ConstantModifier(3),
                new DisadvantageModifier()
            } },
            new object[] { new IJoinModifier[] {
                new ConstantModifier(-2),
                new ConstantModifier(2),
                new DiceWithMultiplierModifier(-2, 2),
                new DiceWithMultiplierModifier(2, 2),
                new AdvantageModifier(),
                new DisadvantageModifier()
            } }
        };

        [Theory]
        [MemberData(nameof(BuildRollFromModifiersThrowData))]
        public void BuildRollFromJoinModifiers_Should_ThrowArgumentException_When_ModifiersHasMoreThanOneAdvantageTypeModifier(IJoinModifier[] modifiers)
        {
            Assert.Throws<ArgumentException>(() => RollHelper.BuildRollFromJoinModifiers(_baseDie, modifiers));
        }
    }
}
