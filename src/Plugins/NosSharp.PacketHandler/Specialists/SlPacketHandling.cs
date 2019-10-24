using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Enums;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Specialists.Args;
using ChickenAPI.Packets.ClientPackets.Specialists;
using ChickenAPI.Packets.Enumerations;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Specialists
{
    public class SlPacketHandling : GenericGamePacketHandlerAsync<SpTransformPacket>
    {
        public SlPacketHandling(ILogger log) : base(log)
        {
        }

        protected override Task Handle(SpTransformPacket packet, IPlayerEntity player)
        {
            switch (packet.Type)
            {
                case SlPacketType.WearSp:
                    return player.EmitEventAsync(new SpTransformEvent
                    {
                        Wait = packet.Header.StartsWith("#") && player.Session.Account.Authority < AuthorityType.GameMaster
                    });

                case SlPacketType.ChangePoints:
                    return player.EmitEventAsync(new SpChangePointsEvent
                    {
                        Attack = packet.SpecialistDamage.GetValueOrDefault(),
                        Defense = packet.SpecialistDefense.GetValueOrDefault(),
                        Element = packet.SpecialistElement.GetValueOrDefault(),
                        HpMp = packet.SpecialistHP.GetValueOrDefault()
                    });
            }

            return Task.CompletedTask;
        }
    }
}