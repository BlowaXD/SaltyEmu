using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace Toolkit.Generators.FromPackets
{
    public class ShopItemGenerator
    {

        private static readonly Logger Log = Logger.GetLogger<ShopItemGenerator>();

        private readonly List<ShopItemDto> _shopItems = new List<ShopItemDto>();

        public void Generate(string filePath)
        {
            var shopService = Container.Instance.Resolve<IShopService>();
            var shopItemService = Container.Instance.Resolve<IShopItemService>();
            string[] lines = File.ReadAllText(filePath, Encoding.Default).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int counter = 0;
            byte type = 0;

            foreach (string line in lines)
            {
                try
                {
                    string[] currentPacket = line.Split('\t', ' ');
                    switch (currentPacket[0])
                    {
                        case "n_inv":
                            short npcId = short.Parse(currentPacket[2]);
                            if (!shopService.GetByMapNpcId(npcId).Any())
                            {
                                continue;
                            }

                            for (int i = 5; i < currentPacket.Length; i++)
                            {
                                string[] item = currentPacket[i].Split('.');
                                if (item.Length != 5 && item.Length != 6)
                                {
                                    continue;
                                }
                                var shopItem = new ShopItemDto
                                {
                                    ShopId = shopService.GetByMapNpcId(npcId).FirstOrDefault()?.Id ?? 0,
                                    Type = type,
                                    Slot = byte.Parse(item[1]),
                                    ItemId = long.Parse(item[2]),
                                };
                                if (item.Length == 6)
                                {
                                    shopItem.Rare = sbyte.Parse(item[3]);
                                    shopItem.Upgrade = byte.Parse(item[4]);
                                }

                                if (_shopItems.Any(s => s.ItemId.Equals(shopItem.ItemId) && s.ShopId.Equals(shopItem.ShopId)) || shopItemService.GetByShopId(shopItem.ShopId).Any(s => s.ItemId.Equals(shopItem.ItemId)))
                                {
                                    continue;
                                }

                                _shopItems.Add(shopItem);
                                counter++;
                            }
                            break;

                        case "shopping":
                            type = byte.Parse(currentPacket[1]);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log.Error("[PARSE]", e);
                    Log.Warn(line);
                }
            }
            shopItemService.Save(_shopItems);
            Log.Info(string.Format("SHOP_ITEMS_PARSED", counter));
        }
    }
}