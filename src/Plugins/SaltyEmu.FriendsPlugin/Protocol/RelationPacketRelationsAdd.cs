using System.Collections.Generic;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Protocol;

namespace SaltyEmu.FriendsPlugin.Protocol
{
    public class RelationPacketRelationsAdd : BaseBroadcastedPacket
    {
        public IEnumerable<RelationDto> Relations { get; set; }
    }
}