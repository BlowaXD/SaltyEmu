using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Old.Game.Client.Movement;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Move
{
    public class WalkPacketHandling : GenericGamePacketHandlerAsync<WalkPacket>
    {
        protected override async Task Handle(WalkPacket packet, IPlayerEntity player)
        {
            if (player.Position.X == packet.XCoordinate && player.Position.Y == packet.YCoordinate)
            {
                return;
            }

            if (player.Speed < packet.Speed)
            {
                return;
            }

            player.Position.X = packet.XCoordinate;
            player.Position.Y = packet.YCoordinate;
            player.Speed = packet.Speed;

            await player.SendPacketAsync(player.GenerateCondPacket());
            await player.BroadcastAsync(player.GenerateMvPacket());
        }
    }
}