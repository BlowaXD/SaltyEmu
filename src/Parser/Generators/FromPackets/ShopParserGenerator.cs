using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Data.AccessLayer.Map;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace Toolkit.Generators.FromPackets
{
    public class ShopParserGenerator
    {

        private static readonly Logger Log = Logger.GetLogger<ShopParserGenerator>();

        private readonly List<ShopDto> _shops = new List<ShopDto>();

        public void Generate(string filePath)
        {
            var shopService = Container.Instance.Resolve<IShopService>();
            var mapNpcService = Container.Instance.Resolve<IMapNpcService>();
            string[] lines = File.ReadAllText(filePath, Encoding.Default).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int counter = 0;

            foreach (string line in lines)
            {
                try
                {
                    string[] currentPacket = line.Split('\t', ' ');
                    if (currentPacket[0] != "shop" || mapNpcService.GetById(long.Parse(currentPacket[2])) == null || _shops.Any(s => s.MapNpcId == long.Parse(currentPacket[2])) || shopService.GetByMapNpcId(int.Parse(currentPacket[2])).Any())
                    {
                        continue;
                    }

                    var shop = new ShopDto
                    {
                        Name = string.Join(" ", currentPacket.Skip(5)),
                        MapNpcId = int.Parse(currentPacket[2]),
                        MenuType = byte.Parse(currentPacket[4]),
                        ShopType = byte.Parse(currentPacket[5])

                    };
                    counter++;
                    _shops.Add(shop);
                }
                catch (Exception e)
                {
                    Log.Error("[PARSE]", e);
                    Log.Warn(line);
                }
            }
            shopService.Save(_shops);
            Log.Info(string.Format("SHOP_PARSED", counter));
        }
    }
}