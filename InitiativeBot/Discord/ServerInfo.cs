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

        public ServerInfo(ulong channelId, ulong messageId)
        {
            ChannelId = channelId;
            MessageId = messageId;
            InitiativeList = new InitiativeList();
        }

        public string GetDiscordMessage()
        {
            string message = Constatns.Message.InitiativeListMessageCommonPart;

            foreach(var player in InitiativeList.Players)
            {
                if (player.State == InitiativeBot.InitiativeList.Player.PlayerState.unactive)
                    message += "||";
                message += $"`[{player.Initiative,2}]` {player.Name}";
                if (player.State == InitiativeBot.InitiativeList.Player.PlayerState.unactive)
                    message += "||";
            }

            if(InitiativeList.Players.Count == 0)
            {
                message += Constatns.Message.EmptyListMessage;
            }

            return message;
        }
    }
}
