using System.Threading.Tasks;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FriendsPlugin.Protocol;

namespace SaltyEmu.RelationService.Handling
{
    public class GetRelationsByCharacterIdHandler : GenericIpcRequestHandler<GetRelationsByCharacterId>
    {
        private readonly IRelationDao _repository;

        public GetRelationsByCharacterIdHandler(IRelationDao repository)
        {
            _repository = repository;
        }

        protected override async Task Handle(GetRelationsByCharacterId request)
        {
            await request.ReplyAsync(new GetRelationsByCharacterId.Response
            {
                Relations = (await _repository.GetRelationsByCharacterId(request.CharacterId))
            });
        }
    }
}