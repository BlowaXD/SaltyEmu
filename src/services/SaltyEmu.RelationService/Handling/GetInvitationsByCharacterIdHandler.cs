using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FriendsPlugin.Protocol;

namespace SaltyEmu.RelationService.Handling
{
    public class GetInvitationsByCharacterIdHandler : GenericIpcRequestHandler<GetRelationsInvitationByCharacterId, GetRelationsInvitationByCharacterId.Response>
    {
        private readonly IRelationInvitationDao _repository;

        public GetInvitationsByCharacterIdHandler(IRelationInvitationDao repository)
        {
            _repository = repository;
        }

        protected override async Task<GetRelationsInvitationByCharacterId.Response> Handle(GetRelationsInvitationByCharacterId request)
        {
            Log.Warn("I'm in GetInvitation");
            return new GetRelationsInvitationByCharacterId.Response
            {
                Invitations = (await _repository.GetAsync()).Where(s => s.TargetId == request.CharacterId)
            };
        }
    }
}