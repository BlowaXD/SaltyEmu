using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;
using SaltyEmu.IpcPlugin.Protocol;

namespace SaltyEmu.IpcPlugin.Communicators
{
    public class PendingRequest
    {
        public IIpcRequest Request { get; set; }
        public TaskCompletionSource<IIpcResponse> Response { get; set; }
    }
}