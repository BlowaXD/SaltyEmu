using System;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FriendsPlugin.Protocol;

namespace SaltyEmu.RelationService.Handling
{
    public class RemoveRelationsHandler : GenericIpcPacketHandler<RemoveRelations>
    {
        private readonly IRelationDao _relations;

        public RemoveRelationsHandler(IRelationDao relations)
        {
            _relations = relations;
        }

        protected override Task Handle(RemoveRelations request)
        {
            return _relations.DeleteByIdsAsync(request.Relations.Select(s => s.Id));
        }
    }
}