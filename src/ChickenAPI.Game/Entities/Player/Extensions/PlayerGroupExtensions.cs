using ChickenAPI.Game.Groups;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class PlayerGroupExtensions
    {
        public static void JoinGroup(this IPlayerEntity player, GroupDto group)
        {
            if (player.HasGroup || group == null)
            {
                return;
            }

            player.Group = group;
            player.ActualiseUiGroupIcons();
        }
    }
}