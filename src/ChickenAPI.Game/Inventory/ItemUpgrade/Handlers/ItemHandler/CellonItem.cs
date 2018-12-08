using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers;
using ChickenAPI.Game.Shops.Extensions;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Handler
{
    public class CellonItem
    {
        [ItemUpgradeHandler(UpgradePacketType.CellonItem)]
        public static void CellonItemNpc(IPlayerEntity player, ItemUpgradeEventArgs e)
        {
            if (e.CellonItem == null)
            {
                return;
            }
            if (e.SecondItem == null)
            {
                return;
            }

            if (e.CellonItem.Item.EffectValue > e.SecondItem.Item.MaxCellonLvl)
            {
                player.SendTopscreenMessage("CELLON_LEVEL_TOO_HIGH", MsgPacketType.White);
                player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow);
                return;
            }

            if (e.SecondItem.Item.MaxCellon <= e.SecondItem.EquipmentOptions.Count)
            {
                player.SendTopscreenMessage("CELLON_FULL", MsgPacketType.White);
                player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow);
                return;
            }

            int gold;
            switch (e.CellonItem.Item.EffectValue)
            {
                case 1:
                    gold = 700;
                    break;
                case 2:
                    gold = 1400;
                    break;
                case 3:
                    gold = 3000;
                    break;
                case 4:
                    gold = 5000;
                    break;
                case 5:
                    gold = 10000;
                    break;
                case 6:
                    gold = 20000;
                    break;
                case 7:
                    gold = 32000;
                    break;
                case 8:
                    gold = 58000;
                    break;
                case 9:
                    gold = 95000;
                    break;
                default:
                    return;
            }

            if (player.Character.Gold < gold)
            {
                player.SendTopscreenMessage("NOT_ENOUGH_GOLD", MsgPacketType.White);
                player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow);
                return;
            }

            player.EmitEvent(new CellonItemEventArgs
            {
                Jewelry = e.SecondItem,
                Cellon = e.CellonItem,
                GoldAmount = gold
            });
        }

    }
}