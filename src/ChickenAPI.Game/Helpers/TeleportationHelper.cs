using System;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Helpers
{
    public static class TeleportationHelper
    {
        private static readonly IMapManager MapManager = new Lazy<IMapManager>(() => ChickenContainer.Instance.Resolve<IMapManager>()).Value;

        public static Task TeleportTo(this IPlayerEntity player, IMapLayer layer, short x, short y)
        {
            if (player.CurrentMap == layer)
            {
                return player.TeleportTo(x, y);
            }

            player.Position.X = x;
            player.Position.Y = y;
            player.TransferEntity(layer);
            return Task.CompletedTask;
        }


        public static Task TeleportTo(this IPlayerEntity player, short mapId, short x, short y)
        {
            return player.TeleportTo(MapManager.GetBaseMapLayer(mapId), x, y);
        }

        public static Task TeleportTo(this IPlayerEntity player, short x, short y)
        {
            // improve that
            player.Position.X = x;
            player.Position.Y = y;
            return player.BroadcastAsync(player.GenerateTpPacket(x, y));
        }
    }
}