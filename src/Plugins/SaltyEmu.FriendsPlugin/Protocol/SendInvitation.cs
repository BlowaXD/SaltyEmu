using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Protocol;

namespace SaltyEmu.FriendsPlugin.Protocol
{
    public class SendInvitation : BaseBroadcastedPacket
    {
        public RelationInvitationDto Invitation { get; set; }
    }
}