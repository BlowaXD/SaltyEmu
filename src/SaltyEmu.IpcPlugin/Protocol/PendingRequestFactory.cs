using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;
using SaltyEmu.IpcPlugin.Communicators;

namespace SaltyEmu.IpcPlugin.Protocol
{
    public class PendingRequestFactory : IPendingRequestFactory
    {
        public PendingRequest Create(IIpcRequest request)
        {
            return new PendingRequest
            {
                Request = request,
                Response = new TaskCompletionSource<IIpcResponse>()
            };
        }
    }
}