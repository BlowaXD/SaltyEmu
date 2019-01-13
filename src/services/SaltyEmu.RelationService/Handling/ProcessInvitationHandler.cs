using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Relations;
using ChickenAPI.Enums.Game.Relations;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FriendsPlugin.Protocol;

namespace SaltyEmu.RelationService.Handling
{
    public class ProcessInvitationHandler : GenericIpcRequestHandler<ProcessInvitation>
    {
        private readonly ISynchronizedRepository<RelationDto> _relations;
        private readonly ISynchronizedRepository<RelationInvitationDto> _repository;
        private readonly ICharacterService _characterService;

        public ProcessInvitationHandler(ISynchronizedRepository<RelationInvitationDto> repository, ISynchronizedRepository<RelationDto> relations, ICharacterService characterService)
        {
            _relations = relations;
            _repository = repository;
            _characterService = characterService;
        }

        protected override async Task Handle(ProcessInvitation request)
        {
            RelationInvitationDto tmp = await _repository.GetByIdAsync(request.InvitationId);

            if (tmp == null)
            {
                return;
            }

            await _repository.DeleteByIdAsync(request.InvitationId);
            IEnumerable<CharacterDto> characters = await _characterService.GetByIdsAsync(new[] { tmp.OwnerId, tmp.TargetId });


            CharacterDto owner = await _characterService.GetByIdAsync(tmp.OwnerId);
            CharacterDto target = await _characterService.GetByIdAsync(tmp.TargetId);

            RelationDto[] relations =
            {
                new RelationDto
                {
                    Id = Guid.NewGuid(),
                    OwnerId = tmp.OwnerId,
                    TargetId = tmp.TargetId,
                    Type = tmp.RelationType,
                    Name = target.Name,
                },
                new RelationDto
                {
                    Id = Guid.NewGuid(),
                    OwnerId = tmp.TargetId,
                    TargetId = tmp.OwnerId,
                    Type = tmp.RelationType,
                    Name = owner.Name
                },
            };
            if (request.ProcessType == RelationInvitationProcessType.Accept)
            {
                await _relations.SaveAsync(relations);
            }

            await request.ReplyAsync(new ProcessInvitation.Response
            {
                Relation = request.ProcessType == RelationInvitationProcessType.Accept ? new List<RelationDto>(relations) : new List<RelationDto>()
            });
        }
    }
}