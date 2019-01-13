using System.Collections.Generic;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Protocol;

namespace SaltyEmu.FriendsPlugin.Protocol
{
    public class GetRelationsInvitationByCharacterId : BaseRequest
    {
        public class Response : BaseResponse
        {
            public IEnumerable<RelationInvitationDto> Invitations { get; set; }
        }

        public long CharacterId { get; set; }
    }
}