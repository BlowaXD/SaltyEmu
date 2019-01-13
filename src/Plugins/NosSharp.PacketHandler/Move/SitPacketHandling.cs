using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Movements.Events;
using ChickenAPI.Packets.Game.Client.Movement;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Move
{
    public class SitPacketHandling : GenericGamePacketHandlerAsync<SitPacket>
    {
        protected override Task Handle(SitPacket packet, IPlayerEntity player)
        {
            foreach (SitSubPacket u in packet.Users)
            {
                if (u.UserType == 1 && u.UserId != player.Character.Id)
                {
                    return Task.CompletedTask;
                }

                //TODO: rest on mate
            }

            return player.EmitEventAsync(new TriggerSitEvent
            {
                ChildsId = packet.Users.Select(s => s.UserId)
            });
        }
    }
}