using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FriendsPlugin.Protocol;

namespace SaltyEmu.RelationService.Handling
{
    public class GetInvitationsByCharacterIdHandler : GenericIpcRequestHandler<GetRelationsInvitationByCharacterId>
    {
        private readonly ISynchronizedRepository<RelationInvitationDto> _repository;

        public GetInvitationsByCharacterIdHandler(ISynchronizedRepository<RelationInvitationDto> repository)
        {
            _repository = repository;
        }

        protected override async Task Handle(GetRelationsInvitationByCharacterId request)
        {
            await request.ReplyAsync(new GetRelationsInvitationByCharacterId.Response
            {
                Invitations = (await _repository.GetAsync()).Where(s => s.TargetId == request.CharacterId)
            });
        }
    }
}