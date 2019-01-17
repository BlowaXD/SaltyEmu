using System;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Protocol
{
    public class BaseRequest : IIpcRequest
    {
        private Guid _id;

        public IIpcServer Server { get; set; }

        public Guid Id
        {
            get => _id == Guid.Empty ? _id = Guid.NewGuid() : _id;
            set => _id = value;
        }

        public Task ReplyAsync<T>(T response) where T : IIpcResponse
        {
            response.RequestId = Id;
            return Server.ResponseAsync(response);
        }
    }
}