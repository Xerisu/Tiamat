using InitiativeBot.InitiativeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    internal class ServerInfo
    {
        public ulong ChannelId { get; }

        public ulong MessageId { get; }

        public IInitiativeList InitiativeList { get; }

        public int ThisIsOnlyATestVariableAndNothingMore { get; set; } = 0;

        public ServerInfo(ulong channelId, ulong messageId)
        {
            ChannelId = channelId;
            MessageId = messageId;
            InitiativeList = new InitiativeList();
        }

        public string GetDiscordMessage()
        {
            ThisIsOnlyATestVariableAndNothingMore++;

            string message = Constatns.Message.InitiativeListMessageCommonPart;
            message += $"{ThisIsOnlyATestVariableAndNothingMore}\n";

            foreach(var player in InitiativeList.Players)
            {
                if (player.State == InitiativeBot.InitiativeList.Player.PlayerState.unactive)
                    message += "||";
                message += $"`[{player.Initiative,2}]` {player.Name}";
                if (player.State == InitiativeBot.InitiativeList.Player.PlayerState.unactive)
                    message += "||";
            }

            return message;
        }
    }
}
