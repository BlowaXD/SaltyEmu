using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Groups;
using ChickenAPI.Game.Inventory.Extensions;

namespace ChickenAPI.Game.Player.Extension
{
    public static class PlayerUiExtension
    {
        /// <summary>
        /// Actualizes the gold in the player's inventory (ClientSide)
        /// </summary>
        /// <param name="player"></param>
        public static void ActualiseUiGold(this IPlayerEntity player)
        {
            player.SendPacket(player.GenerateGoldPacket());
        }

        /// <summary>
        /// Actualizes the Sp Bar Points
        /// </summary>
        /// <param name="player"></param>
        public static void ActualiseUiSpPoints(this IPlayerEntity player)
        {
            player.SendPacket(player.GenerateSpPacket());
        }

        /// <summary>
        /// Broadcasts the ReputationPacket to all the players on the map
        /// </summary>
        /// <param name="player"></param>
        public static void ActualiseUiReputation(this IPlayerEntity player)
        {
            player.Broadcast(player.GenerateFdPacket());
        }

        /// <summary>
        /// Actualizes the player ExpBar / Level / HeroLevel
        /// </summary>
        /// <param name="player"></param>
        public static void ActualiseUiExpBar(this IPlayerEntity player)
        {
            player.SendPacket(player.GenerateLevPacket());
        }

        /// <summary>
        /// Actualizes the HpBar at top left corner
        /// </summary>
        /// <param name="player"></param>
        public static void ActualiseUiHpBar(this IPlayerEntity player)
        {
            player.SendPacket(player.GenerateStatPacket());
        }

        /// <summary>
        /// Actualizes the Group Icons + their Group Bars
        /// </summary>
        /// <param name="player"></param>
        public static void ActualiseUiGroupIcons(this IPlayerEntity player)
        {
            player.SendPackets(player.GeneratePstPacket());
        }
    }
}