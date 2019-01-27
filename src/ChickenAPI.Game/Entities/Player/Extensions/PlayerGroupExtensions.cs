using System.Threading.Tasks;
using ChickenAPI.Game.Groups;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class PlayerGroupExtensions
    {
        public static Task JoinGroup(this IPlayerEntity player, Group group)
        {
            if (player.HasGroup || group == null)
            {
                return Task.CompletedTask;
            }

            player.Group = group;
            group.Players.Add(player);
            return Task.CompletedTask;
        }

        public static Task LeaveGroup(this IPlayerEntity player, Group group)
        {
            if (!player.HasGroup || group == null)
            {
                return Task.CompletedTask;
            }

            player.Group = null;
            group.Players.Remove(player);
            return Task.CompletedTask;
        }
    }
}