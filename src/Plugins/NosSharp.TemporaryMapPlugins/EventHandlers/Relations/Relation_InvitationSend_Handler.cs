using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Relations;
using ChickenAPI.Game.Relations.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers.Relations
{
    public class Relation_InvitationSend_Handler : GenericEventPostProcessorBase<RelationInvitationSendEvent>
    {
        private readonly IRelationService _relationService;
        private readonly ICharacterService _characterService;

        public Relation_InvitationSend_Handler(ICharacterService characterService, IRelationService relationService)
        {
            _characterService = characterService;
            _relationService = relationService;
        }

        protected override async Task Handle(RelationInvitationSendEvent e, CancellationToken cancellation)
        {
            CharacterDto targetCharacter = await _characterService.GetActiveByNameAsync(e.TargetCharacterName);
            await _relationService.RelationInviteAsync(e.Sender.Id, targetCharacter.Id, e.ExpectedRelationType);
        }
    }
}