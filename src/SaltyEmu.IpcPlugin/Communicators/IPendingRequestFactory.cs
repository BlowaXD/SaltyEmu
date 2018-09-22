using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.IpcPlugin.Communicators
{
    public interface IPendingRequestFactory
    {
        PendingRequest Create(IIpcRequest request);
    }
}