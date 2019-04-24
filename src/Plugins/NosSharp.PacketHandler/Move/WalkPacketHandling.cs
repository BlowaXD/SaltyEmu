using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Packets.ClientPackets.Movement;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Move
{
    public class WalkPacketHandling : GenericGamePacketHandlerAsync<WalkPacket>
    {
        public WalkPacketHandling(ILogger log) : base(log)
        {
        }

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
            player.Speed = (byte)packet.Speed;

            await player.SendPacketAsync(player.GenerateCondPacket());
            await player.BroadcastAsync(player.GenerateMvPacket());
        }
    }
}