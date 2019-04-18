using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class IvnPacketExtension
    {
        public static IvnPacket GenerateEmptyIvnPacket(this IPlayerEntity player, InventoryType type, short slot) =>
            new IvnPacket
            {
                InventoryType = type,
                Slot = slot,
                ItemId = -1,
                Upgrade = 0,
                Rare = 0,
                SpStoneUpgrade = 0
            };

        public static IvnPacket GenerateIvnPacket(this ItemInstanceDto itemInstance)
        {
            switch (itemInstance.Type)
            {
                case InventoryType.Specialist:
                    return new IvnPacket
                    {
                        InventoryType = itemInstance.Type,
                        ItemId = itemInstance.Item.Id,
                        Slot = itemInstance.Slot,
                        Upgrade = itemInstance.Upgrade,
                        Rare = itemInstance.Rarity,
                        SpStoneUpgrade = itemInstance.SpecialistUpgrade
                    };

                case InventoryType.Equipment:
                    return new IvnPacket
                    {
                        InventoryType = itemInstance.Type,
                        ItemId = itemInstance.ItemId,
                        Slot = itemInstance.Slot,
                        Rare = itemInstance.Rarity,
                        Upgrade = itemInstance.Upgrade
                    };

                case InventoryType.Main:
                case InventoryType.Etc:
                    return itemInstance.GenerateMainIvnPacket();

                default:
                    //Log.Info($"{itemInstance.Type} not implemented");
                    return null;
            }
        }

        private static IvnPacket GenerateMainIvnPacket(this ItemInstanceDto itemInstance) => new IvnPacket
        {
            InventoryType = itemInstance.Type,
            Slot = itemInstance.Slot,
            ItemId = itemInstance.ItemId,
            Rare = itemInstance.Amount,
            Upgrade = 0
        };
    }
}