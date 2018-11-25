using System.Collections.Generic;

namespace SaltyEmu.Communication.Protocol.RepositoryPacket
{
    public class RepositoryDeleteRequest<TKey> : BaseRequest
    {
        public IEnumerable<TKey> ObjectId { get; set; }
    }
}