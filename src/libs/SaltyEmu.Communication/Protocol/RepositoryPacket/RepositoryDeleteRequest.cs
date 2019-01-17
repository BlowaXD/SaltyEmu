using System;
using System.Collections.Generic;
using SaltyEmu.Communication.Protocol.Attributes;

namespace SaltyEmu.Communication.Protocol.RepositoryPacket
{
    [PacketRequest(typeof(RepositoryDeleteRequest<>))]
    public class RepositoryDeleteRequest<TKey> : BaseRequest
    {
        public class Response : BaseResponse
        {
            public IEnumerable<TKey> DeletedItems { get; set; }
        }

        public IEnumerable<TKey> ObjectIds { get; set; }
    }
}