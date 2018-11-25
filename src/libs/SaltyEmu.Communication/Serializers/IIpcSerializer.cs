using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Serializers
{
    public interface IIpcSerializer
    {
        byte[] Serialize<T>(T packet);

        T Deserialize<T>(byte[] buffer);
    }
}