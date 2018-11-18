using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Specialists.Args;
using ChickenAPI.Packets.Game.Client.Specialists;

namespace NosSharp.PacketHandler.Specialists
{
    public class SlPacketHandling
    {
        private readonly Logger Log = Logger.GetLogger<SlPacketHandling>();

        public static void OnPacket(SlPacket packet, IPlayerEntity session)
        {
            switch (packet.Type)
            {
                case SlPacketType.WearSp:
                    session.EmitEvent(new SpTransformEvent
                    {
                    });
                    break;
                case SlPacketType.ChangePoints:
                    session.EmitEvent(new SpChangePointsEvent
                    {
                        Attack = packet.SpecialistDamage,
                        Defense = packet.SpecialistDefense,
                        Element = packet.SpecialistElement,
                        HpMp = packet.SpecialistHp,
                    });
                    return;
            }
        }
    }
}