using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Utils
{
    public interface IPendingRequestFactory
    {
        PendingRequest Create(IIpcRequest request);
    }
}