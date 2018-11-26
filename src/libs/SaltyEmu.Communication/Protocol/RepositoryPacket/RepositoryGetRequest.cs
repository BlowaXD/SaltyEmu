using System;
using System.Collections.Generic;
using System.Text;

namespace SaltyEmu.Communication.Protocol.RepositoryPacket
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    internal sealed class RepositoryGetResponse<TObject> : BaseResponse
    {
        public IEnumerable<TObject> Objects { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public sealed class RepositoryGetRequest<TKey> : BaseRequest
    {
        /// <summary>
        /// if this property is null, means that you want to request every objects
        /// </summary>
        public IEnumerable<TKey> ObjectIds { get; set; }
    }
}