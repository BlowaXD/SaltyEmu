using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Shop;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Inventory.Args;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Player.Extension;
using ChickenAPI.Game.Shops.Args;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Game.Shops.Extensions;
using ChickenAPI.Game.Skills.Extensions;
using ChickenAPI.Packets.Game.Client.Shops;

namespace ChickenAPI.Game.Shops
{
    public class ShopEventHandler : EventHandlerBase
    {
        private static readonly IRandomGenerator _randomGenerator = new Lazy<IRandomGenerator>(() => ChickenContainer.Instance.Resolve<IRandomGenerator>()).Value;

        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(ShopGetInformationEvent), typeof(ShopBuyEvent), typeof(ShopSellEvent)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case ShopGetInformationEvent getinfos:
                    SendInformations(getinfos, entity);
                    break;
                case ShopBuyEvent buy:
                    HandleBuyRequest(entity as IPlayerEntity, buy);
                    break;
                case ShopSellEvent sell:
                    HandleSellRequest(entity as IPlayerEntity, sell);
                    break;
            }
        }

        private static void SendInformations(ShopGetInformationEvent getinfos, IEntity entity)
        {
            if (!(entity is IPlayerEntity player))
            {
                // not a player, no need to send packets
                return;
            }

            int typeshop = 0;

            var tmp = new StringBuilder();
            double percent = 1.0;

            switch (player.GetDignityIcon())
            {
                case CharacterDignity.BluffedNameOnly:
                    percent = 1.1;
                    typeshop = 110;
                    break;
                case CharacterDignity.NotQualifiedFor:
                    percent = 1.2;
                    typeshop = 120;
                    break;
                case CharacterDignity.Useless:
                case CharacterDignity.StupidMinded:
                    percent = 1.5;
                    typeshop = 150;
                    break;
            }

            foreach (ShopItemDto itemInfo in getinfos.Shop.Items.Where(s => s.Type == getinfos.Type))
            {
                if (typeshop == 0)
                {
                    typeshop = 100;
                }

                tmp.Append(' ');
                double price = itemInfo.Item.ReputPrice > 0 ? itemInfo.Item.ReputPrice : itemInfo.Item.Price * percent;
                byte color = itemInfo.Color != 0 ? itemInfo.Item.Color : itemInfo.Item.BasicUpgrade;
                int rare = itemInfo.Item.Type != InventoryType.Equipment ? -1 : itemInfo.Rare;

                tmp.Append((byte)itemInfo.Item.Type);
                tmp.Append('.');
                tmp.Append(itemInfo.Slot);
                tmp.Append('.');
                tmp.Append(itemInfo.ItemId);
                tmp.Append('.');
                tmp.Append(rare);
                tmp.Append('.');
                if (itemInfo.Item.Type == InventoryType.Equipment)
                {
                    tmp.Append(color);
                    tmp.Append('.');
                }

                tmp.Append(price);
            }

            player.SendPacket(new NInvPacket
            {
                ShopList = tmp.ToString().Trim(),
                ShopSkills = getinfos.Shop.Skills.Where(s => s.Type.Equals(getinfos.Type)).Select(s => s.SkillId).ToList(),
                ShopType = typeshop,
                Unknown = 0,
                VisualId = getinfos.Shop.MapNpcId,
                VisualType = 2
            });
        }

        private static void HandleBuyRequest(IPlayerEntity player, ShopBuyEvent shopBuy)
        {
            switch (shopBuy.Type)
            {
                case VisualType.Character:
                    IPlayerEntity shop = player.CurrentMap.GetEntitiesByType<IPlayerEntity>(VisualType.Character).FirstOrDefault(s => s.Character.Id == shopBuy.OwnerId);
                    if (shop == null)
                    {
                        return;
                    }

                    HandlePlayerShopBuyRequest(player, shopBuy, shop);
                    break;
                case VisualType.Npc:
                    INpcEntity npc = player.CurrentMap.GetEntitiesByType<INpcEntity>(VisualType.Npc).FirstOrDefault(s => s.MapNpc.Id == shopBuy.OwnerId);
                    if (npc == null || !(npc is INpcEntity npcEntity))
                    {
                        return;
                    }

                    Shop npcShop = npcEntity.Shop;
                    if (npcShop.Skills.Any())
                    {
                        HandleNpcSkillBuyRequest(player, shopBuy, npcShop);
                    }
                    else
                    {
                        HandleNpcItemBuyRequest(player, shopBuy, npcShop);
                    }

                    break;
            }
        }

        private static void HandleNpcSkillBuyRequest(IPlayerEntity player, ShopBuyEvent shopBuy, Shop shop)
        {
            ShopSkillDto skillShop = shop.Skills.FirstOrDefault(s => s.SkillId == shopBuy.Slot);
            if (skillShop == null)
            {
                return;
            }

            // check use sp

            // check skill cooldown
            if (player.SkillComponent.CooldownsBySkillId.Any(s => s.Item2 == shopBuy.Slot))
            {
                return;
            }

            // check skill already exists in player skills
            if (player.SkillComponent.Skills.ContainsKey(shopBuy.Slot))
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

        private static void HandleNpcItemBuyRequest(IPlayerEntity player, ShopBuyEvent shopBuy, Shop shop)
        {
            ShopItemDto item = shop.Items.FirstOrDefault(s => s.Slot == shopBuy.Slot);
            short amount = shopBuy.Amount;

            if (item == null || amount <= 0)
            {
                return;
            }

            // check diginity
            double percent = 1.0;
            switch (player.GetDignityIcon())
            {
                case CharacterDignity.BluffedNameOnly:
                    percent = 1.10;
                    break;
                case CharacterDignity.NotQualifiedFor:
                    percent = 1.20;
                    break;
                case CharacterDignity.Useless:
                case CharacterDignity.StupidMinded:
                    percent = 1.5;
                    break;
            }

            bool isReputBuy = item.Item.ReputPrice > 0;
            long price = isReputBuy ? item.Item.ReputPrice : item.Item.Price;
            price *= amount;
            sbyte rare = item.Rare;
            if (item.Item.Type == 0)
            {
                amount = 1;
            }

            if (!isReputBuy && price < 0 && price * percent > player.Character.Gold)
            {
                player.SendPacket(player.GenerateShopMemoPacket(SMemoPacketType.FailNpc, player.GetLanguage(ChickenI18NKey.YOU_DONT_HAVE_ENOUGH_GOLD)));
                return;
            }

            if (isReputBuy)
            {
                if (price > player.Character.Reput)
                {
                    player.SendPacket(player.GenerateShopMemoPacket(SMemoPacketType.FailNpc, player.GetLanguage(ChickenI18NKey.YOU_DONT_HAVE_ENOUGH_REPUTATION)));
                    return;
                }

                // generate a random rarity
                byte ra = (byte)_randomGenerator.Next(100);

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

            bool canAddItem = player.Inventory.CanAddItem(item.Item, amount);
            if (!canAddItem)
            {
                player.SendPacket(player.GenerateShopMemoPacket(SMemoPacketType.FailNpc, player.GetLanguage(ChickenI18NKey.YOU_DONT_HAVE_ENOUGH_SPACE_IN_INVENTORY)));
                return;
            }

            // add item to inventory
            var itemFactory = ChickenContainer.Instance.Resolve<IItemInstanceFactory>();
            ItemInstanceDto newitem = itemFactory.CreateItem(item.ItemId, amount, (byte)rare);

            if (isReputBuy)
            {
                player.Character.Reput -= price;
                player.ActualiseUiReputation();
                // player.SendPacket "reput decreased"
            }
            else
            {
                player.Character.Gold -= (long)(price * percent);
                player.SendPacket(player.GenerateGoldPacket());
            }

            player.EmitEvent(new InventoryAddItemEvent
            {
                ItemInstance = newitem
            });
        }

        private static void HandlePlayerShopBuyRequest(IPlayerEntity player, ShopBuyEvent shopBuy, IPlayerEntity shop)
        {
            // todo
        }

        private static void HandleSellRequest(IPlayerEntity player, ShopSellEvent shopSell)
        {
            // todo
        }
    }
}