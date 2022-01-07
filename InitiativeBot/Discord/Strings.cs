using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public static class Strings
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
                public const string HelpResponseMessage = "Tiamat usage:\nTo configure channel use commad `/tiamat-setup`.\nThen just use new channel lol.";
            }
        }

        public const string TiamatChannelName = "tiamat-initiative-list";
    }
}
