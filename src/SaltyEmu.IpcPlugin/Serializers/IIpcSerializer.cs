using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Serializers
{
    public interface IIpcSerializer
    {
        byte[] Serialize(IIpcPacket packet);

        IIpcPacket Deserialize(byte[] buffer);
    }
}