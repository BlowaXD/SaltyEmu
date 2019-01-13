using System.Collections.Generic;

namespace SaltyEmu.Communication.Protocol.RepositoryPacket
{
    public class RepositoryDeleteRequest<TKey> : BaseRequest
    {
        public class Response : BaseResponse
        {
            public IEnumerable<TKey> DeletedItems { get; set; }
        }

        public IEnumerable<TKey> ObjectIds { get; set; }
    }
}