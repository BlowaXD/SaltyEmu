using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.AccessLayer.Shop;
using ChickenAPI.Data.TransferObjects.Shop;

namespace NosSharp.Parser.Generators.FromPackets
{
    public class RecipeGenerator
    {

        private static readonly Logger Log = Logger.GetLogger<RecipeGenerator>();

        private readonly List<RecipeDto> _recipes = new List<RecipeDto>();
        private readonly List<RecipeItemDto> _recipeItems = new List<RecipeItemDto>();

        public void Generate(string filePath)
        {
            var shopService = Container.Instance.Resolve<IShopService>();
            var recipeService = Container.Instance.Resolve<IRecipeService>();
            var recipeItemService = Container.Instance.Resolve<IRecipeItemService>();
            string[] lines = File.ReadAllText(filePath, Encoding.Default).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int counter = 0;
            int counter2 = 0;
            long shopId = 0;
            long itemId = 0;
            _recipes.AddRange(recipeService.Get());

            foreach (string line in lines)
            {
                try
                {
                    string[] currentPacket = line.Split('\t', ' ');
                    switch (currentPacket[0])
                    {
                        case "n_run":
                            shopId = shopService.GetByMapNpcId(long.Parse(currentPacket[4])).FirstOrDefault()?.Id ?? -1;
                            break;
                        case "m_list" when currentPacket[1] == "2" || currentPacket[1] == "4":
                            for (int i = 2; i < currentPacket.Length - 1; i++)
                            {
                                long itmId = long.Parse(currentPacket[i]);
                                if (shopId == -1 || _recipes.Any(r => r.ShopId == shopId || r.ItemId == itemId))
                                {
                                    continue;
                                }

                                _recipes.Add(new RecipeDto
                                {
                                    ItemId = itmId,
                                    ShopId = shopId
                                });
                                counter++;
                            }

                            break;
                        case "m_list" when currentPacket[1] == "3" || currentPacket[1] == "5":
                            for (int i = 3; i < currentPacket.Length - 1; i += 2)
                            {
                                RecipeDto rec = _recipes.FirstOrDefault(r => r.ItemId == itemId && r.ShopId == shopId);
                                if (rec == null || recipeItemService.GetByRecipeId(rec.Id).Any(r => r.ItemId == long.Parse(currentPacket[i]) && r.Amount == byte.Parse(currentPacket[i + 1])))
                                {
                                    continue;
                                }

                                rec.Amount = byte.Parse(currentPacket[2]);
                                _recipeItems.Add(new RecipeItemDto
                                {
                                    ItemId = long.Parse(currentPacket[i]),
                                    Amount = byte.Parse(currentPacket[i + 1]),
                                    RecipeId = rec.Id
                                });
                                counter2++;
                            }
                            itemId = -1;
                            break;
                        case "pdtse":
                            itemId = long.Parse(currentPacket[2]);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log.Error("[PARSE]", e);
                    Log.Warn(line);
                }
            }

            recipeService.Save(_recipes);
            Log.Info(string.Format("RECIPE_PARSED", counter));
            recipeItemService.Save(_recipeItems);
            Log.Info(string.Format("RECIP_ITEMS_PARSED", counter2));
        }
    }
}