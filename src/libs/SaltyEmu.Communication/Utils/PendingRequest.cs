using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Utils
{
    public class PendingRequest
    {
        public IIpcRequest Request { get; set; }
        public TaskCompletionSource<IIpcResponse> Response { get; set; }
    }
}