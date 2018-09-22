using ChickenAPI.Core.IPC.Protocol;
using SaltyEmu.IpcPlugin.Communicators;

namespace SaltyEmu.IpcPlugin.Protocol
{
    public interface IPendingRequestFactory
    {
        PendingRequest Create(IIpcRequest request);
    }
}