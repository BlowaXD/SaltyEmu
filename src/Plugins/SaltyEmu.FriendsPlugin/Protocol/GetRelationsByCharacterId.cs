using System.Collections.Generic;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Protocol;

namespace SaltyEmu.FriendsPlugin.Protocol
{
    public class GetRelationsByCharacterId : BaseRequest
    {
        public class Response : BaseResponse
        {
            public IEnumerable<RelationDto> Relations { get; set; }
        }

        public long CharacterId { get; set; }
    }

}