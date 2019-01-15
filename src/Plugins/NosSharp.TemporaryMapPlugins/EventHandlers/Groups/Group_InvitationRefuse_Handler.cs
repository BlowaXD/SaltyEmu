using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Groups.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Group_InvitationRefuse_Handler : GenericEventPostProcessorBase<GroupInvitationRefuseEvent>
    {
        protected override Task Handle(GroupInvitationRefuseEvent e, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}