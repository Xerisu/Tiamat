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
                public const string CommandName = "tiamat-setup";
                public const string CommandDescription = "Setup text channel as Tiamat's channel";
            }

            public static class TiamatHelp
            {
                public const string CommandName = "tiamat-help";
                public const string CommandDescription = "Shows help about Tiamat.";
                public const string HelpResponseMessage = "Tiamat usage:\nTo configure channel use commad `/tiamat-setup`.\nThen just use new channel lol.";
            }
        }
    }
}
