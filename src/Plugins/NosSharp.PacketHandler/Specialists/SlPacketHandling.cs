using System.Threading.Tasks;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Specialists.Args;
using ChickenAPI.Packets.Game.Client.Specialists;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Specialists
{
    public class SlPacketHandling : GenericGamePacketHandlerAsync<SlPacket>
    {
        protected override Task Handle(SlPacket packet, IPlayerEntity player)
        {
            switch (packet.Type)
            {
                case SlPacketType.WearSp:
                    return player.EmitEventAsync(new SpTransformEvent());
                case SlPacketType.ChangePoints:
                    return player.EmitEventAsync(new SpChangePointsEvent
                    {
                        Attack = packet.SpecialistDamage,
                        Defense = packet.SpecialistDefense,
                        Element = packet.SpecialistElement,
                        HpMp = packet.SpecialistHp
                    });
            }

            return Task.CompletedTask;
        }
    }
}