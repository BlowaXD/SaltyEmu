using System;
using System.Collections.Generic;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Protocol;

namespace SaltyEmu.FriendsPlugin.Protocol
{
    public class RemoveRelations : BaseBroadcastedPacket
    {
        public IEnumerable<RelationDto> Relations { get; set; }
    }
}