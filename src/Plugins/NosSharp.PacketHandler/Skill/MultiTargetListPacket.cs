using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client.Battle;
using NosSharp.PacketHandler.Utils;

namespace NosSharp.PacketHandler.Skill
{
    public class MultiTargetListPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<MultiTargetListPacketHandling>();

        public static void OnPacketReceived(MultiTargetListPacket packet, IPlayerEntity player)
        {
            Log.Debug(packet.OriginalContent);
        }
    }
}