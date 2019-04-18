using System;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Protocol
{
    public class BaseRequest : ISyncRpcRequest
    {
        private Guid _id;

        public IRpcServer Server { get; set; }

        public Guid Id
        {
            get => _id == Guid.Empty ? _id = Guid.NewGuid() : _id;
            set => _id = value;
        }

        public Task ReplyAsync<T, TRequest>(T response) where T : ISyncRpcResponse
        {
            response.RequestId = Id;
            return Server.ResponseAsync(response, typeof(TRequest));
        }
    }
}