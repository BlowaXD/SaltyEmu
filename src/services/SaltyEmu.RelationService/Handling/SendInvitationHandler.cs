using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FriendsPlugin.Protocol;

namespace SaltyEmu.RelationService.Handling
{
    public class SendInvitationHandler : GenericIpcPacketHandler<SendInvitation>
    {
        private readonly ISynchronizedRepository<RelationInvitationDto> _invitations;

        public SendInvitationHandler(ISynchronizedRepository<RelationInvitationDto> invitation)
        {
            _invitations = invitation;
        }

        protected override async Task Handle(SendInvitation request)
        {
            Log.Info($"Saving {request.Invitation}");
            await _invitations.SaveAsync(request.Invitation);
        }
    }
}