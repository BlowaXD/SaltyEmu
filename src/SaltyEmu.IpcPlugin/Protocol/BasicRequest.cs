using System;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.IpcPlugin.Protocol
{
    public class BaseRequest : IIpcRequest
    {
        private Guid _id;

        public Guid Id
        {
            get => _id == Guid.Empty ? _id = Guid.NewGuid() : _id;
            set => _id = value;
        }

        public IIpcServer Server { get; set; }

        public Task ReplyAsync<T>(T response) where T : IIpcResponse
        {
            response.RequestId = Id;
            return Server.ResponseAsync<T>(response);
        }
    }
}