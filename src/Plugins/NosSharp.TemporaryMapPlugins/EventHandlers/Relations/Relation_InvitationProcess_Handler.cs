using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Relations;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Player.Extension;
using ChickenAPI.Game.Relations.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers.Relations
{
    public class Relation_InvitationProcess_Handler : GenericEventPostProcessorBase<RelationInvitationProcessEvent>
    {
        private readonly IRelationService _relationService;

        public Relation_InvitationProcess_Handler(IRelationService relationService)
        {
            _relationService = relationService;
        }

        protected override async Task Handle(RelationInvitationProcessEvent e, CancellationToken cancellation)
        {
            IEnumerable<RelationInvitationDto> invitations = await _relationService.GetPendingInvitationByCharacterIdAsync(e.Sender.Id);
            RelationInvitationDto invitation = invitations.FirstOrDefault(s => s.TargetId == e.Sender.Id && s.OwnerId == e.TargetCharacterId);
            if (invitation == null)
            {
                // no invitation to process
                return;
            }

            await _relationService.RelationInviteProcessAsync(invitation.Id, e.Type);
        }
    }
}