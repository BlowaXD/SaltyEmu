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
        public Guid Id { get; set; }

        public IIpcServer Server { get; set; }

        public Task ReplyAsync(IIpcResponse response)
        {
            return Server.ResponseAsync(response);
        }
    }
}