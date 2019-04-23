using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Specialists.Args;
using ChickenAPI.Packets.Old.Game.Client.Specialists;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Specialists
{
    public class SlPacketHandling : GenericGamePacketHandlerAsync<SlPacket>
    {
        protected override Task Handle(SlPacket packet, IPlayerEntity player)
        {
            string[] packetsplit = packet.OriginalContent.Split(' ', '^');

            switch (packet.Type)
            {
                case SlPacketType.WearSp:
                    return player.EmitEventAsync(new SpTransformEvent
                    {
                        Wait = packetsplit[1].ElementAt(0) == '#' ? false : player.Session.Account.Authority >= AuthorityType.GameMaster ? false : true
                    });

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