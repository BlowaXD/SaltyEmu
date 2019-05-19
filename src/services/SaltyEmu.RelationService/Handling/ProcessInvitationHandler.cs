using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Relations;
using ChickenAPI.Enums.Game.Relations;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FriendsPlugin.Protocol;

namespace SaltyEmu.RelationService.Handling
{
    public class ProcessInvitationHandler : GenericIpcRequestHandler<ProcessInvitation, ProcessInvitation.Response>
    {
        private readonly IRelationDao _relations;
        private readonly IRelationInvitationDao _repository;
        private readonly ICharacterService _characterService;


        public ProcessInvitationHandler(ILogger log, IRelationDao relations, IRelationInvitationDao repository, ICharacterService characterService) : base(log)
        {
            _relations = relations;
            _repository = repository;
            _characterService = characterService;
        }

        protected override async Task<ProcessInvitation.Response> Handle(ProcessInvitation request)
        {
            RelationInvitationDto tmp = await _repository.GetByIdAsync(request.InvitationId);

            if (tmp == null)
            {
                return new ProcessInvitation.Response
                {
                    Relation = new List<RelationDto>()
                };
            }

            await _repository.DeleteByIdAsync(request.InvitationId);

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
                    Name = target?.Name ?? tmp.TargetId.ToString()
                },
                new RelationDto
                {
                    Id = Guid.NewGuid(),
                    OwnerId = tmp.TargetId,
                    TargetId = tmp.OwnerId,
                    Type = tmp.RelationType,
                    Name = owner?.Name ?? tmp.OwnerId.ToString()
                },
            };
            if (request.ProcessType == RelationInvitationProcessType.Accept)
            {
                await _relations.SaveAsync(relations);
            }

            return new ProcessInvitation.Response
            {
                Relation = request.ProcessType == RelationInvitationProcessType.Accept ? new List<RelationDto>(relations) : new List<RelationDto>()
            };
        }
    }
}