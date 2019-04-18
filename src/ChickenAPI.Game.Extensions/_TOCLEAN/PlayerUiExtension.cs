using System.Threading.Tasks;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Families.Extensions;
using ChickenAPI.Game.Groups.Extensions;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.Relations.Extensions;
using ChickenAPI.Game.Skills.Extensions;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class PlayerUiExtension
    {
        /// <summary>
        /// Actualizes the wear stuffs in "P" panel
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Task ActualizeUiWearPanel(this IPlayerEntity player) => player.SendPacketAsync(player.GenerateEquipPacket());

        /// <summary>
        ///     Actualizes the gold in the player's inventory (ClientSide)
        /// </summary>
        /// <param name="player"></param>
        public static Task ActualizeUiGold(this IPlayerEntity player) => player.SendPacketAsync(player.GenerateGoldPacket());

        /// <summary>
        ///     Actualizes the Sp Bar Points
        /// </summary>
        /// <param name="player"></param>
        public static Task ActualiseUiSpPoints(this IPlayerEntity player) => player.SendPacketAsync(player.GenerateSpPacket());

        /// <summary>
        ///     Broadcasts the ReputationPacket to all the players on the map
        /// </summary>
        /// <param name="player"></param>
        public static Task ActualizeUiReputation(this IPlayerEntity player) =>
            Task.WhenAll(
                player.SendPacketAsync(player.GenerateFdPacket()),
                player.BroadcastExceptSenderAsync(player.GenerateInPacket()),
                player.BroadcastExceptSenderAsync(player.GenerateGidxPacket())
            );

        /// <summary>
        ///     Actualizes the given player inventory slot
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <param name="slot"></param>
        public static Task ActualizeUiInventorySlot(this IPlayerEntity player, InventoryType type, short slot)
        {
            ItemInstanceDto tmp = player.Inventory.GetItemFromSlotAndType(slot, type);

            return player.SendPacketAsync(tmp == null ? player.GenerateEmptyIvnPacket(type, slot) : tmp.GenerateIvnPacket());
        }

        /// <summary>
        ///     Actualizes the player ExpBar / Level / HeroLevel
        /// </summary>
        /// <param name="player"></param>
        public static Task ActualizeUiExpBar(this IPlayerEntity player) => player.SendPacketAsync(player.GenerateLevPacket());

        /// <summary>
        /// Actualize the size
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Task ActualizeUiSize(this IPlayerEntity player) => player.BroadcastAsync(player.GenerateCharScPacket());

        /// <summary>
        ///     Actualizes the HpBar at top left corner
        /// </summary>
        /// <param name="player"></param>
        public static Task ActualizeUiHpBar(this IPlayerEntity player) => player.SendPacketAsync(player.GenerateStatPacket());

        /// <summary>
        ///     Actualizes :
        ///     Speed
        ///     CanMove
        ///     CanAttack
        /// </summary>
        /// <param name="player"></param>
        public static Task ActualizePlayerCondition(this IPlayerEntity player) => player.SendPacketAsync(player.GenerateCondPacket());

        public static Task ActualizeUiBlackList(this IPlayerEntity player) => player.SendPacketAsync(player.GenerateBlIinitPacket());

        public static Task ActualizeUiFriendList(this IPlayerEntity player) => player.SendPacketAsync(player.GenerateFInitPacket());

        public static Task ActualizeUiSkillList(this IPlayerEntity player) => player.SendPacketAsync(player.GenerateSkiPacket());

        public static Task ActualizeUiQuicklist(this IPlayerEntity player) => player.SendPacketsAsync(player.GenerateQuicklistPacket());

        public static Task ActualizeGroupList(this IPlayerEntity player) => player.BroadcastAsync(player.GeneratePidxPacket());

        /// <summary>
        ///     Actualize Group Icons on Entity Group Join / Leave and Mates Join/Leave
        /// </summary>
        /// <param name="player"></param>
        public static Task ActualizeUiGroupIcons(this IPlayerEntity player) => player.SendPacketAsync(player.GeneratePInitPacket());

        public static Task ActualizeMateListInPlayerPanel(this IPlayerEntity player) =>
            Task.WhenAll(
                player.ActualizePartnersListInPlayerPanel(),
                player.ActualizeNosmatesListInPlayerPanel()
            );

        public static Task ActualizePartnersListInPlayerPanel(this IPlayerEntity player) => player.SendPacketsAsync(player.GenerateScP());

        public static Task ActualizeNosmatesListInPlayerPanel(this IPlayerEntity player) => player.SendPacketsAsync(player.GenerateScN());

        /// <summary>
        ///     Supposedly casted all X seconds to actualize your team stats
        /// </summary>
        /// <param name="player"></param>
        public static Task ActualizeUiGroupStats(this IPlayerEntity player) => player.SendPacketsAsync(player.GeneratePstPackets());
    }
}