using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Groups.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Group_InvitationAccept_Handler : GenericEventPostProcessorBase<GroupInvitationAcceptEvent>
    {
        protected override Task Handle(GroupInvitationAcceptEvent e, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}