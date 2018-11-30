using System.Collections.Generic;

namespace SaltyEmu.Communication.Protocol.RepositoryPacket
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public sealed class RepositorySaveResponse<TObject> : BaseResponse
    {
        public IEnumerable<TObject> Objects { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public sealed class RepositorySaveRequest<TObject> : BaseRequest
    {
        /// <summary>
        /// if this property is null, means that you want to request every objects
        /// </summary>
        public IEnumerable<TObject> Objects { get; set; }
    }
}