using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Shops;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Game.Shops.Extensions;
using ChickenAPI.Packets.ClientPackets.Shops;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Shop;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Shops
{
    public class PersonalShopCreationHandling : GenericGamePacketHandlerAsync<MShopPacket>
    {
        public PersonalShopCreationHandling(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(MShopPacket packet, IPlayerEntity player)
        {
            switch (packet.Type)
            {
                case CreateShopPacketType.Create:
                    try
                    {
                        //ShopPlayerShopCreateEvent e = ParseMShopPacket(packet.ItemList, player);
                        var e = new ShopPlayerShopCreateEvent
                        {
                            ShopItems = packet.ItemList.Select(s => new PersonalShopItem
                            {
                                Slot = s.Slot,
                                Price = s.Price,
                                ItemInstance = player.Inventory.GetItemFromSlotAndType(s.Slot, s.Type),
                                Quantity = s.Amount
                            }).ToList()
                        };
                        if (e?.ShopItems.Any() == true)
                        {
                            await player.EmitEventAsync(e);
                        }
                        else
                        {
                            await player.SendPacketAsync(player.GenerateShopEndPacket(ShopEndPacketType.PersonalShop));
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error("[M_SHOP]", e);
                    }

                    break;
                case CreateShopPacketType.Close:
                    await player.ClosePersonalShopAsync();
                    player.IsSitting = false;
                    await player.ActualizePlayerCondition();
                    return;
                case CreateShopPacketType.Open:
                    await player.SendPacketAsync(new IshopPacket());
                    return;
                default:
                    return;
            }
        }

        private static ShopPlayerShopCreateEvent ParseMShopPacket(string data, IPlayerEntity player)
        {
            var tmp = new ShopPlayerShopCreateEvent
            {
                ShopItems = new List<PersonalShopItem>()
            };

            string[] packetsplit = data.Split(' ');
            PocketType[] type = new PocketType[20];
            long[] gold = new long[20];
            short[] slot = new short[20];
            byte[] qty = new byte[20];
            string shopname = string.Empty;
            short shopSlot = 0;

            if (packetsplit.Length > 79)
            {
                for (short j = 0, i = 0; j < 79; j += 4, i++)
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

                    if (!itemFromSlotAndType.Item.IsTradable || itemFromSlotAndType.BoundCharacterId.HasValue)
                    {
                        // await session.SendPacketAsync(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("SHOP_ONLY_TRADABLE_ITEMS"), 10));
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

            for (int i = 80; i < packetsplit.Length; i++)
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

            return tmp;
        }
    }
}