using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.GuriHandling.Args;
using ChickenAPI.Packets.Game.Client.Player;

namespace NosSharp.PacketHandler
{
    public class GuriPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<GuriPacketHandling>();

        public static void GuriPacket(ClientGuriPacket packet, IPlayerEntity session)
        {
            session.EmitEvent(new GuriEventArgs
            {
                EffectId = packet.Type,
                Data = (short)packet.Data
            });
        }
    }
}