using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public static class Constatns
    {
        public static class Commands
        {
            public static class TiamatSetup
            {
                public const string CommandName = "create-channel";
                public const string CommandDescription = "Creates new channel for Tiamat in case old one was removed.";
                public const string SetupResponseMessage = "Channel created and ready to use.";
            }

            public static class TiamatHelp
            {
                public const string CommandName = "help";
                public const string CommandDescription = "Shows help about Tiamat.";
                public const string HelpResponseMessage = "Tiamat usage:\nTo configure channel use commad `/setup`.\nThen just use new channel lol.";
            }
        }

        public const string TiamatChannelName = "tiamat-initiative-list";

        public static class Message
        {
            public const string IntermediateStepMessage = "This message is an intermediate step in configuring bot in this server.";
            public const string InitiativeListMessageCommonPart = "**_Initiative List_**:\n";
            public const string EmptyListMessage = "_Add new players by using command `/join {name} {modifiers}`_";
            public static class Button
            {
                public const string Label = "Next Turn";
                public const string Id = "next-turn-button";
                public const string Emote = "➡️"; //Must be 1 pure UTF-8 emote or nothing/whitespace
                public const ButtonStyle Style = ButtonStyle.Secondary;

                public const string ButtonResponseMessage = "Moved to the next player.";
            }
        }
    }
}
