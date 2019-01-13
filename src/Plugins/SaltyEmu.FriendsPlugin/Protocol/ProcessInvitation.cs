using System;
using System.Collections.Generic;
using ChickenAPI.Data.Relations;
using ChickenAPI.Enums.Game.Relations;
using SaltyEmu.Communication.Protocol;

namespace SaltyEmu.FriendsPlugin.Protocol
{
    public class ProcessInvitation : BaseRequest
    {
        public class Response : BaseResponse
        {
            public List<RelationDto> Relation { get; set; }
        }

        public Guid InvitationId { get; set; }
        public RelationInvitationProcessType ProcessType { get; set; }
    }
}