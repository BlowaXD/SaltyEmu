using System;
using System.Linq;
using System.Text;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Shops.Args;
using ChickenAPI.Game.Features.Shops.Packets;

namespace ChickenAPI.Game.Features.Shops
{
    public class ShopEventHandler : EventHandlerBase
    {
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
                tmp.Append((byte)itemInfo.Item.EquipmentSlot);
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
            if (shop.Skills.All(s => s.SkillId != buy.Slot))
            {

            }

            // check use sp

            // check skill cooldown
            // check skill already exists in player skills
            // check skill price
            // check skill cp
            // check skill class
            // check skill minimum level
            // check skill upgrades

            // player.Character.Gold -= skillinfo.Price;
            // player.SendPacket(player.GenerateGold());
            // player.SendPacket(player.GenerateSki());
            // player.SendPacket(player.GenerateQuicklist());
            // player.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("SKILL_LEARNED"), 0));
            // player.SendPacket(Session.Character.GenerateLev());
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
                var random = new Random();
                byte ra = (byte)random.Next(100);

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