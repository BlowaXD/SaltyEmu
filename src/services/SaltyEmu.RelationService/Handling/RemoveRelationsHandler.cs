using System;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FriendsPlugin.Protocol;

namespace SaltyEmu.RelationService.Handling
{
    public class RemoveRelationsHandler : GenericAsyncRpcRequestHandler<RemoveRelations>
    {
        private readonly IRelationDao _relations;

        public RemoveRelationsHandler(ILogger log, IRelationDao relations) : base(log) => _relations = relations;

        protected override Task Handle(RemoveRelations request)
        {
            return _relations.DeleteByIdsAsync(request.Relations.Select(s => s.Id));
        }
    }
}