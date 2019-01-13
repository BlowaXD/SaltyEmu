using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Extensions.UserInterface
{
    public static class TargetHpBarExtensions
    {
        /// <summary>
        ///     The Hpbar at the left of the screen with player & mates icons
        /// </summary>
        /// <param name="player"></param>
        public static void ActualizeUiTargetHpBar(this IPlayerEntity player)
        {
            if (!player.HasTarget)
            {
                return;
            }

            player.SendPacket(player.Target.GenerateStPacket());
        }
    }
}