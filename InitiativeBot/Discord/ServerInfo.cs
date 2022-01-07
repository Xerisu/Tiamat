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
    }
}
