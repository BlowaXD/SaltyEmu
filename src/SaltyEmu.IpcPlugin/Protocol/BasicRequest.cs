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

        public async Task ReplyAsync(IIpcResponse response)
        {
            await ChickenContainer.Instance.Resolve<IIpcServer>().ResponseAsync(response);
        }
    }
}