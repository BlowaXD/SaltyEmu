using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets;

namespace NosSharp.PacketHandler.Utils
{
    public abstract class BasePacketHandling<TPacket> where TPacket : PacketBase
    {
        protected static readonly Logger Log = Logger.GetLogger<TPacket>();
        public abstract void OnPacketReceived(TPacket packet, IPlayerEntity player);
    }
}