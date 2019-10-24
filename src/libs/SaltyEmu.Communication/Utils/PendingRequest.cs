using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Utils
{
    public class PendingRequest
    {
        public ISyncRpcRequest Request { get; set; }
        public TaskCompletionSource<ISyncRpcResponse> Response { get; set; }
    }
}