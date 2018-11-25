using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Inventory.Extensions;

namespace ChickenAPI.Game.Player.Extension
{
    public static class PlayerUiExtension
    {
        public static void ActualiseUiGold(this IPlayerEntity player)
        {
            player.SendPacket(player.GenerateGoldPacket());
        }

        public static void ActualiseUiSpPoints(this IPlayerEntity player)
        {
            player.SendPacket(player.GenerateSpPacket());
        }

        public static void ActualiseUiExpBar(this IPlayerEntity player)
        {
            player.SendPacket(player.GenerateLevPacket());
        }

        public static void ActualiseUiHpBar(this IPlayerEntity player)
        {
            player.SendPacket(player.GenerateStatPacket());
        }
    }
}