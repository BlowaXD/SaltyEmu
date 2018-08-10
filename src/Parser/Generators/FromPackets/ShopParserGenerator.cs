using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Data.AccessLayer.Map;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.TransferObjects.Map;
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
            Dictionary<long, MapNpcDto> npcs = mapNpcService.Get().ToDictionary(s => s.Id, s => s);
            Dictionary<long, ShopDto> shops = shopService.Get().ToDictionary(s => s.Id, s => s);
            string[] splitters = { "\r\n", "\r", "\n" };
            string[] lines = File.ReadAllText(filePath, Encoding.Default).Split(splitters, StringSplitOptions.RemoveEmptyEntries);
            int counter = 0;
            int tmpp = 0;

            foreach (string line in lines.Where(s => s.StartsWith("shop") && !s.StartsWith("shopping") && !s.StartsWith("shopclose")))
            {
                try
                {
                    tmpp++;
                    string[] currentPacket = line.Split('\t', ' ');
                    long mapnpcid = long.Parse(currentPacket[2]);
                    if (!npcs.ContainsKey(mapnpcid) || _shops.Any(s => s.MapNpcId == mapnpcid) || shops.Any(s => s.Value.MapNpcId == mapnpcid))
                    {
                        continue;
                    }

                    var shop = new ShopDto
                    {
                        Name = string.Join(" ", currentPacket.Skip(6)).Trim(),
                        MapNpcId = mapnpcid,
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
        }
    }
}