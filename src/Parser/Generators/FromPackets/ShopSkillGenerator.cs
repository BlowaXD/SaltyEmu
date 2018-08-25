using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace Toolkit.Generators.FromPackets
{
    public class ShopSkillGenerator
    {
        private static readonly Logger Log = Logger.GetLogger<ShopSkillGenerator>();

        private readonly List<ShopSkillDto> _shopSkills = new List<ShopSkillDto>();

        public void Generate(string filePath)
        {
            var shopService = ChickenContainer.Instance.Resolve<IShopService>();
            var shopSkillService = ChickenContainer.Instance.Resolve<IShopSkillService>();
            string[] lines = File.ReadAllText(filePath, Encoding.Default).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int counter = 0;
            byte type = 0;

            foreach (string line in lines.Where(s => s.StartsWith("n_inv") || s.StartsWith("shopping")))
            {
                try
                {
                    string[] currentPacket = line.Split('\t', ' ');
                    switch (currentPacket[0])
                    {
                        case "shopping":
                            if (currentPacket.Length > 3)
                            {
                                type = byte.Parse(currentPacket[1]);
                            }

                            break;
                        case "n_inv":
                            short npcId = short.Parse(currentPacket[2]);
                            if (!shopService.GetByMapNpcId(npcId).Any())
                            {
                                continue;
                            }

                            for (int i = 5; i < currentPacket.Length; i++)
                            {
                                if (currentPacket[i].Contains("."))
                                {
                                    continue;
                                }

                                var shopItem = new ShopSkillDto
                                {
                                    ShopId = shopService.GetByMapNpcId(npcId).FirstOrDefault()?.Id ?? 0,
                                    Type = type,
                                    Slot = (byte)(i - 5),
                                    SkillId = long.Parse(currentPacket[i])
                                };

                                if (shopItem.ShopId == 0)
                                {
                                    continue;
                                }


                                if (_shopSkills.Any(s => s.SkillId == shopItem.SkillId && s.ShopId == shopItem.ShopId) ||
                                    shopSkillService.GetByShopId(shopItem.ShopId).Any(s => s.SkillId == shopItem.SkillId))
                                {
                                    continue;
                                }

                                _shopSkills.Add(shopItem);
                                counter++;
                            }

                            break;
                    }
                }
                catch (Exception e)
                {
                    Log.Error("[PARSE]", e);
                    Log.Warn(line);
                }
            }

            shopSkillService.Save(_shopSkills);
        }
    }
}