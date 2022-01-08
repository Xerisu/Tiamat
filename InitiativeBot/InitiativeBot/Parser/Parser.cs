using InitiativeBot.Parser.JoinModifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InitiativeBot.Parser
{
    /// <summary>
    /// Parsing strings in a custom way
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Parse string with modifiers of the roll (from join command) into list of modifiers.
        /// <para>There are no checks if modifiers make sense - only if they are valid tokens.</para>
        /// </summary>
        /// <param name="modifiersString">String with all modifiers</param>
        /// <returns>Array with all parsed modifiers in the order they were in the string</returns>
        /// <exception cref="ArgumentException">Could not parse modifiers</exception>
        public static IJoinModifier[] ParseJoinModifiersString(string modifiersString)
        {
            List<IJoinModifier> modifiers = new();

            string str = Regex.Replace(modifiersString, @"\s", "").ToLowerInvariant();
            string actualToken = "";

            for(int i = 0; i < str.Length; i++)
            {
                actualToken += str[i];

                if(actualToken == "adv" || actualToken == "dis")
                {
                    modifiers.Add(GetModifierFromString(actualToken));
                    actualToken = "";
                }
                else if(i < str.Length - 1)
                {
                    // New token can start with +, - or a (for advantage); dis is separate
                    if (new char[] { '-', '+', 'a' }.Contains(str[i+1])) {
                        modifiers.Add(GetModifierFromString(actualToken));
                        actualToken = "";
                    }

                    // If we have at least 2 more tokens, and next 2 tokens are precisely "di" we
                    //   end actual token so the next one should be "dis"
                    //   in other cases 'd' can be part of dice (like 2d8) or "adv"
                    if(i < str.Length - 2 && str[i+1] == 'd' && str[i+2] == 'i') 
                    {
                        modifiers.Add(GetModifierFromString(actualToken));
                        actualToken = "";
                    }
                }
            }

            if(actualToken != "")
            {
                modifiers.Add(GetModifierFromString(actualToken));
            }

            return modifiers.ToArray();
        }

        /// <summary>
        /// Gets modifier form string
        /// </summary>
        /// <param name="modifier">One modifier as a string</param>
        /// <returns>Proper modifier</returns>
        /// <exception cref="ArgumentException">No modifier matched given string</exception>
        private static IJoinModifier GetModifierFromString(string modifier)
        {
            if (modifier == "adv")
                return new AdvantageModifier();
            if (modifier == "dis")
                return new DisadvantageModifier();
            if (Int32.TryParse(modifier, out int constant))
                return new ConstantModifier(constant);

            var diceModifierMatch = Regex.Match(modifier, @"^(\+|-)?(\d*)d(\d+)$");
            if(!diceModifierMatch.Success)
            {
                throw new ArgumentException($"Modifier {modifier} for the join command is not valid.", nameof(modifier));
            }

            int multiplier = Int32.TryParse(diceModifierMatch.Groups[2]?.Value, out int mult) ? mult : 1;
            if (diceModifierMatch.Captures[0] != null && diceModifierMatch.Groups[1].Value == "-")
                multiplier *= -1;
            int dice = Int32.TryParse(diceModifierMatch.Groups[3]?.Value, out int d) ? (d > 1 ? d : 1) : 1;
            return new DiceWithMultiplierModifier(multiplier, dice);
        }
    }
}
