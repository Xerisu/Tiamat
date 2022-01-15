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
            InitiativeList = new InitiativeList(new List<InitiativeBot.InitiativeList.Player.Player>(), new InitiativeBot.RNG.RNG());
        }

        public string GetDiscordMessage()
        {
            string message = InitiativeList.Round == 0 ? Constants.Message.RoundZeroMessage : string.Format(Constants.Message.InCombatRoundMessage, InitiativeList.Round);
            message += Constants.Message.InitiativeListMessageCommonPart;

            for (int i = 0; i < InitiativeList.Players.Count; i++)
            {
                var player = InitiativeList.Players[i];
                if (player.State == InitiativeBot.InitiativeList.Player.PlayerState.unactive)
                    message += "||";
                if (InitiativeList.Round != 0 && i == InitiativeList.ActivePlayerIndex)
                    message += "**";
                message += $"`[{player.Initiative,2}]` {player.Name}";
                if (InitiativeList.Round != 0 && i == InitiativeList.ActivePlayerIndex)
                    message += "**";
                if (player.State == InitiativeBot.InitiativeList.Player.PlayerState.unactive)
                    message += "||";
                message += "\n";
            }

            if(InitiativeList.Players.Count == 0)
            {
                message += Constants.Message.EmptyListMessage;
            }

            return message;
        }
    }
}
