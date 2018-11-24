using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Utils
{
    public class PendingRequestFactory : IPendingRequestFactory
    {
        public PendingRequest Create(IIpcRequest request) => new PendingRequest
        {
            Request = request,
            Response = new TaskCompletionSource<IIpcResponse>()
        };
    }
}