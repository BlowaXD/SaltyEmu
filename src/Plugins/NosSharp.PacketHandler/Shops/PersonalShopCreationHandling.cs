using System;
using System.Linq;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Shops;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Packets.Game.Client.Shops;
using ChickenAPI.Packets.Game.Server.Shop;

namespace NosSharp.PacketHandler.Shops
{
    public class PersonalShopCreationHandling
    {
        public static void OnMShopPacketReceived(MShopPacket packet, IPlayerEntity player)
        {
            switch (packet.PacketType)
            {
                case MShopPacketType.OpenShop:
                    try
                    {
                        ShopPlayerShopCreateEvent e = ParseMShopPacket(packet.PacketData, player);
                        if (e.ShopItems.Any())
                        {
                            player.EmitEvent(e);
                        }
                    }
                    catch (Exception e)
                    {
                        return;
                    }

                    break;
                case MShopPacketType.CloseShop:
                    player.SendPacket(new ShopEndPacket { PacketType = ShopEndPacketType.PersonalShop });
                    return;
                case MShopPacketType.OpenDialog:
                    player.SendPacket(new IShopPacket());
                    return;
                default:
                    return;
            }
        }

        public static ShopPlayerShopCreateEvent ParseMShopPacket(string data, IPlayerEntity player)
        {
            var tmp = new ShopPlayerShopCreateEvent();

            string[] packetsplit = data.Split(' ');
            InventoryType[] type = new InventoryType[20];
            long[] gold = new long[20];
            short[] slot = new short[20];
            byte[] qty = new byte[20];
            string shopname = string.Empty;
            short shopSlot = 0;

            if (packetsplit.Length > 82)
            {
                for (short j = 3, i = 0; j < 82; j += 4, i++)
                {
                    Enum.TryParse(packetsplit[j], out type[i]);
                    short.TryParse(packetsplit[j + 1], out slot[i]);
                    byte.TryParse(packetsplit[j + 2], out qty[i]);
                    long.TryParse(packetsplit[j + 3], out gold[i]);
                    if (gold[i] < 0)
                    {
                        // should not input unsigned amount
                        return null;
                    }

                    if (qty[i] <= 0)
                    {
                        continue;
                    }

                    ItemInstanceDto itemFromSlotAndType = player.Inventory.GetItemFromSlotAndType(slot[i], type[i]);
                    if (itemFromSlotAndType == null)
                    {
                        continue;
                    }

                    if (itemFromSlotAndType.Amount < qty[i])
                    {
                        return null;
                    }

                    if (!itemFromSlotAndType.Item.IsTradable || itemFromSlotAndType.BoundCharacterId != 0)
                    {
                        // Session.SendPacket(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("SHOP_ONLY_TRADABLE_ITEMS"), 10));
                        // Session.SendPacket("shop_end 0");
                        return null;
                    }

                    var personalshopitem = new PersonalShopItem
                    {
                        Slot = shopSlot++,
                        Price = gold[i],
                        ItemInstance = itemFromSlotAndType,
                        Quantity = qty[i]
                    };
                    tmp.ShopItems.Add(personalshopitem);
                }
            }

            if (!tmp.ShopItems.Any(s => !s.ItemInstance.Item.IsSoldable || s.ItemInstance.BoundCharacterId != 0))
            {
                for (int i = 83; i < packetsplit.Length; i++)
                {
                    shopname += $"{packetsplit[i]} ";
                }

                // trim shopname
                shopname = shopname.TrimEnd(' ');

                // create default shopname if it's empty
                if (string.IsNullOrWhiteSpace(shopname) || string.IsNullOrEmpty(shopname))
                {
                    shopname = "SHOP_PRIVATE_SHOP";
                }

                // truncate the string to a max-length of 20
                shopname = shopname.Truncate(20);
                tmp.Name = shopname;
            }

            return tmp;
        }
    }
}