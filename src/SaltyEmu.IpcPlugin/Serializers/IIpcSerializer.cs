using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Serializers
{
    public interface IIpcSerializer<T>
    {
        byte[] Serialize(T packet);

        T Deserialize(byte[] buffer);
    }
}