using System;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Inventory.Extensions;
using ChickenAPI.Game.Features.Shops.Args;
using ChickenAPI.Game.Features.Shops.Packets;
using ChickenAPI.Game.Features.Skills.Extensions;

namespace ChickenAPI.Game.Features.Shops
{
    public class ShopEventHandler : EventHandlerBase
    {

        private static IRandomGenerator _randomGenerator;
        private static IRandomGenerator Random =>
            _randomGenerator ?? (_randomGenerator = Container.Instance.Resolve<IRandomGenerator>());
        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case GetShopInformationEventArgs getinfos:
                    SendInformations(getinfos, entity);
                    break;
                case BuyShopEventArgs buy:
                    HandleBuyRequest(entity as IPlayerEntity, buy);
                    break;
                case SellShopEventArgs sell:
                    HandleSellRequest(entity as IPlayerEntity, sell);
                    break;
            }
        }

        private static void SendInformations(GetShopInformationEventArgs getinfos, IEntity entity)
        {
            if (!(entity is IPlayerEntity player))
            {
                // not a player, no need to send packets
                return;
            }

            int typeshop = 0;

            var tmp = new StringBuilder();
            float percent = 1.0f;
            foreach (ShopItemDto itemInfo in getinfos.Shop.Items.Where(s => s.Type == getinfos.Type))
            {
                if (typeshop == 0)
                {
                    typeshop = 100;
                }

                tmp.Append(' ');
                float price = itemInfo.Item.ReputPrice > 0 ? itemInfo.Item.ReputPrice : itemInfo.Item.Price * percent;
                byte color = itemInfo.Color != 0 ? itemInfo.Item.Color : itemInfo.Item.BasicUpgrade;
                int rare = itemInfo.Type == 0 ? itemInfo.Rare : -1;

                tmp.Append(itemInfo.Type);
                tmp.Append('.');
                tmp.Append(itemInfo.Slot);
                tmp.Append('.');
                tmp.Append(itemInfo.ItemId);
                tmp.Append('.');
                tmp.Append(rare);
                tmp.Append('.');
                tmp.Append(color);
                tmp.Append('.');
                tmp.Append(price);
            }

            foreach (ShopSkillDto skill in getinfos.Shop.Skills.Where(s => s.Type.Equals(getinfos.Type)))
            {
                SkillDto skillinfo = skill.Skill;

                tmp.Append(' ');

                if (skill.Type != 0)
                {
                    typeshop = 1;
                    if (skillinfo.Class == (byte)player.Character.Class)
                    {
                        tmp.Append(skillinfo.Id);
                    }
                }
                else
                {
                    tmp.Append(skillinfo.Id);
                }
            }

            player.SendPacket(new NInvPacket
            {
                ShopList = tmp.ToString().Trim(),
                ShopType = typeshop,
                Unknown = 0,
                VisualId = getinfos.Shop.MapNpcId,
                VisualType = 2
            });
        }

        private static void HandleBuyRequest(IPlayerEntity player, BuyShopEventArgs buy)
        {
            switch (buy.Type)
            {
                case VisualType.Character:
                    IPlayerEntity shop = player.EntityManager.GetEntitiesByType<IPlayerEntity>(EntityType.Player).FirstOrDefault(s => s.Character.Id == buy.OwnerId);
                    if (shop == null)
                    {
                        return;
                    }

                    HandlePlayerShopBuyRequest(player, buy, shop);
                    break;
                case VisualType.Npc:
                    INpcEntity npc = player.EntityManager.GetEntitiesByType<INpcEntity>(EntityType.Npc).FirstOrDefault(s => s.MapNpc.Id == buy.OwnerId);
                    if (npc == null || !(npc is NpcEntity npcEntity))
                    {
                        return;
                    }

                    Shop npcShop = npcEntity.Shop;
                    if (npcShop.Skills.Any())
                    {
                        HandleNpcSkillBuyRequest(player, buy, npcShop);
                    }
                    else
                    {
                        HandleNpcItemBuyRequest(player, buy, npcShop);
                    }

                    break;
            }
        }

        private static void HandleNpcSkillBuyRequest(IPlayerEntity player, BuyShopEventArgs buy, Shop shop)
        {
            ShopSkillDto skillShop = shop.Skills.FirstOrDefault(s => s.SkillId == buy.Slot);
            if (skillShop == null)
            {
                return;
            }

            // check use sp
            
            // check skill cooldown
            if (player.Skills.CooldownsBySkillId.Any(s => s.Item2 == buy.Slot))
            {
                return;
            }

            // check skill already exists in player skills
            if (player.Skills.Skills.ContainsKey(buy.Slot))
            {
                return;
            }

            // check skill class
            if ((byte)player.Character.Class != skillShop.Skill.Class)
            {
                return;
            }


            // check skill price
            if (player.Character.Gold < skillShop.Skill.Price)
            {
                return;
            }

            // check skill cp
            if (player.GetCp() < skillShop.Skill.CpCost)
            {
                return;
            }

            // check skill minimum level
            byte minimumLevel = 1;
            switch (player.Character.Class)
            {
                case CharacterClassType.Adventurer:
                    minimumLevel = skillShop.Skill.MinimumAdventurerLevel;
                    break;
                case CharacterClassType.Swordman:
                    minimumLevel = skillShop.Skill.MinimumSwordmanLevel;
                    break;
                case CharacterClassType.Archer:
                    minimumLevel = skillShop.Skill.MinimumArcherLevel;
                    break;
                case CharacterClassType.Magician:
                    minimumLevel = skillShop.Skill.MinimumMagicianLevel;
                    break;
                case CharacterClassType.Wrestler:
                    minimumLevel = skillShop.Skill.MinimumWrestlerLevel;
                    break;
                case CharacterClassType.Unknown:
                    break;
            }

            if (player.Character.JobLevel < minimumLevel)
            {
                return;
            }

            // check skill upgrades

            player.Character.Gold -= skillShop.Skill.Price;
            player.SendPacket(player.GenerateGoldPacket());
            player.SendPacket(player.GenerateSkiPacket());
            player.SendPackets(player.GenerateQuicklistPacket());
            // player.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("SKILL_LEARNED"), 0));
            player.SendPacket(player.GenerateLevPacket());
        }

        private static void HandleNpcItemBuyRequest(IPlayerEntity player, BuyShopEventArgs buy, Shop shop)
        {
            ShopItemDto item = shop.Items.FirstOrDefault(s => s.Slot == buy.Slot);
            if (item == null)
            {
                return;
            }

            // check diginity
            bool isReputBuy = item.Item.ReputPrice > 0;
            long price = isReputBuy ? item.Item.ReputPrice : item.Item.Price;
            sbyte rare = item.Rare;
            double percent = 1.0;
            long amount = buy.Amount;
            if (item.Item.Type == 0)
            {
                amount = 1;
            }

            if (!isReputBuy && price < 0 && price * percent > player.Character.Gold)
            {
                // not enough gold
                return;
            }

            if (isReputBuy)
            {
                if (price > player.Character.Reput)
                {
                    // not enough reputation
                    return;
                }

                // generate a random rarity
                byte ra = (byte)Random.Next(100);

                int[] rareprob = { 100, 100, 70, 50, 30, 15, 5, 1 };
                if (item.Item.ReputPrice == 0)
                {
                    return;
                }

                for (int i = 0; i < rareprob.Length; i++)
                {
                    if (ra <= rareprob[i])
                    {
                        rare = (sbyte)i;
                    }
                }
            }

            bool canAddItem = (buy.Slot % 3) == 0; // todo extension method for inventory
            if (!canAddItem)
            {
                // no available slot
                return;
            }

            // add item to inventory

            if (isReputBuy)
            {
                player.Character.Gold -= (long)(price * percent);
                // player.SendPacket(player.GenerateGoldPacket());
            }
            else
            {
                player.Character.Reput -= price;
                // player.SendPacket(player.GenerateFdPacket());
                // player.SendPacket "reput decreased"
            }
        }

        private static void HandlePlayerShopBuyRequest(IPlayerEntity player, BuyShopEventArgs buy, IPlayerEntity shop)
        {
            throw new NotImplementedException();
        }

        private static void HandleSellRequest(IPlayerEntity player, SellShopEventArgs sell)
        {
            throw new NotImplementedException();
        }
    }
}