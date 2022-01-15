using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public static class Constants
    {
        public static class Commands
        {
            public static class TiamatSetup
            {
                public const string CommandName = "create-channel";
                public const string CommandDescription = "Creates new channel for Tiamat in case old one was removed.";
                public const string ResponseMessage = "Channel created and ready to use.";
            }

            public static class TiamatHelp
            {
                public const string CommandName = "help";
                public const string CommandDescription = "Shows help about Tiamat.";
                public const string ResponseMessage =
@"**/Help Command**

**/create-channel** - Adds the channel  `#tiamat-initiative-list`  in case it was removed or its name got changed

**/join `player-name` `modifiers`** - Adds player to the initiative queue, with an `inactive` state (inactive players are skipped in the current round, and are hidden with ||spoilers||. Player-name can have spaces and special characters. To add modifiers, confirm typing name by pressing `Tab` , choose `modifiers` from the list (or press `Tab` twice) and then type modifiers of your initiative roll
*possible modifiers:*
`adv` - adds advantage to a roll (2d20, drop lowest)
`dis` - adds disadvantage to a roll (2d20, drop highest)
`+<number>` - adds a constant number to a roll, ex. +8
`-<number>` - subtracts a constant number from a roll, ex. -1
`+d<number>` -  adds a dice to a roll, ex. +d8
`-d<number>` - subtracts a dice number from a roll, ex. -d10
You can add or subtract multiple dices, ex. +2d4, -2d10

When you add a player that already exists, it will remove the existing one and add him once again

**/remove `player-name`** - removes player from the queue.

**/clear** - clears the queue and resets the round counter (use it only when the battle finishes)";
            }

            public static class TiamatClear
            {
                public const string CommandName = "clear";
                public const string CommandDescription = "Clears the initiative list.";
                public const string ResponseMessage = "List cleared.";
            }

            public static class TiamatRemove
            {
                public const string CommandName = "remove";
                public const string CommandDescription = "Removes given player from the list.";
                public const string ResponseMessage = "Player {0} removed from the list."; //Should have 1 parameter: name of the player

                public const string PlayerNameParameterName = "player-name";
                public const string PlayerNameParameterDescription = "Name of the player.";
            }

            public static class TiamatJoin
            {
                public const string CommandName = "join";
                public const string CommandDescription = "Adds new player to the list.";
                public const string ResponseMessage = "Player {0} added to the list with modifiers {1}."; //Should have 2 parameters: name of the player, list of modifiers

                public const string PlayerNameParameterName = "player-name";
                public const string PlayerNameParameterDescription = "Name of the player.";

                public const string ModifiersParameterName = "modifiers";
                public const string ModifiersParameterDescription = "Initiative modifiers of the player.";
            }
        }

        public static class BotSettings
        {
            public const string ChannelName = "tiamat-initiative-list";
            public const int DefaultDie = 20;
        }

        public static class Message
        {
            public const string IntermediateStepMessage = "This message is an intermediate step in configuring bot in this server.";
            public const string InitiativeListMessageCommonPart = "**_Initiative List_**:\n";
            public const string EmptyListMessage = "_Add new players by using command `/join {name} {modifiers}`_";
            public const string RoundZeroMessage = "__**Before Combat**__\n";
            public const string InCombatRoundMessage = "__**Round: {0}**__\n"; //Should have 1 parameter: round number

            public static class Button
            {
                public const string Label = "Next Turn";
                public const string Id = "next-turn-button";
                public const string Emote = "➡️"; //Must be 1 pure UTF-8 emote or nothing/whitespace
                public const ButtonStyle Style = ButtonStyle.Secondary;
            }
        }

        public static class Error
        {
            public const string UnknownErrorMessage = "Unknown error :frowning:\nTry to reconfigure bot with `/create-channel` or contact authors.";
            public const string ReconfigureServerErrorMessage = "Server was not properly configured. Try to run `/create-channel`.";
            public const string MissingParameterErrorMessage = "Parameter `{0}` is missing."; //Should have 1 parameter: name of the missing parameter
            public const string InvalidJoinModifiersErrorMessage = "Modifiers are invalid.";
        }
    }
}
