using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Shop;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Shops;
using ChickenAPI.Game.Shops.Events;
using ChickenAPI.Game.Shops.Extensions;
using ChickenAPI.Game.Skills.Extensions;
using ChickenAPI.Game._i18n;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.EventHandlers.Shops
{
    public class Shop_Buy_Event : GenericEventPostProcessorBase<ShopBuyEvent>
    {
        private readonly IRandomGenerator _randomGenerator;

        public Shop_Buy_Event(IRandomGenerator randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        protected override async Task Handle(ShopBuyEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            switch (e.Type)
            {
                case VisualType.Player:
                    IPlayerEntity shop = player.CurrentMap.GetEntitiesByType<IPlayerEntity>(VisualType.Player).FirstOrDefault(s => s.Character.Id == e.OwnerId);
                    if (shop == null)
                    {
                        return;
                    }

                    await HandlePlayerShopBuyRequest(player, e, shop);
                    break;

                case VisualType.Npc:
                    INpcEntity npc = player.CurrentMap.GetEntitiesByType<INpcEntity>(VisualType.Npc).FirstOrDefault(s => s.MapNpc.Id == e.OwnerId);
                    if (npc == null || !(npc is INpcEntity npcEntity))
                    {
                        return;
                    }

                    Shop npcShop = npcEntity.Shop;
                    if (npcShop.Skills.Any())
                    {
                        await HandleNpcSkillBuyRequest(player, e, npcShop);
                    }
                    else
                    {
                        await HandleNpcItemBuyRequest(player, e, npcShop);
                    }

                    break;
            }
        }


        private static Task HandleBuyRequest(IPlayerEntity player, ShopBuyEvent shopBuy)
        {
            return Task.CompletedTask;
        }

        private static async Task HandleNpcSkillBuyRequest(IPlayerEntity player, ShopBuyEvent shopBuy, Shop shop)
        {
            ShopSkillDto skillShop = shop.Skills.FirstOrDefault(s => s.SkillId == shopBuy.Slot);
            if (skillShop == null)
            {
                return;
            }

            // check use sp
            if (player.HasSpWeared)
            {
                return;
            }

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

                case CharacterClassType.MartialArtist:
                    minimumLevel = skillShop.Skill.MinimumWrestlerLevel;
                    break;

                case CharacterClassType.Unknown:
                    break;
            }

            if (skillShop.Skill.MinimumSwordmanLevel == 0 && skillShop.Skill.MinimumArcherLevel == 0 && skillShop.Skill.MinimumMagicianLevel == 0)
            {
                minimumLevel = skillShop.Skill.MinimumAdventurerLevel;
            }

            if (minimumLevel == 0)
            {
                // can't learn the skill
                return;
            }

            // level is required for passive skills
            if (player.Character.Level < minimumLevel && skillShop.Skill.Id <= 200)
            {
                return;
            }

            // job level requirement
            if (player.Character.JobLevel < minimumLevel && skillShop.SkillId >= 200)
            {
                return;
            }

            if (skillShop.SkillId < 200)
            {
                foreach (CharacterSkillDto skill in player.SkillComponent.CharacterSkills.Select(s => s.Value))
                {
                    if (skillShop.Skill.CastId == skill.Skill.CastId && skill.Skill.Id < 200)
                    {
                        player.SkillComponent.CharacterSkills.Remove(skill.Id);
                    }
                }
            }

            // check skill upgrades
            // remove old upgrade
            if (skillShop.SkillId >= 200 && skillShop.Skill.UpgradeSkill != 0)
            {
                CharacterSkillDto oldupgrade = player.SkillComponent.CharacterSkills.FirstOrDefault(s =>
                    s.Value.Skill.UpgradeSkill == skillShop.Skill.UpgradeSkill && s.Value.Skill.UpgradeType == skillShop.Skill.UpgradeType && s.Value.Skill.UpgradeSkill != 0).Value;
                if (oldupgrade != null)
                {
                    player.SkillComponent.CharacterSkills.Remove(oldupgrade.Id);
                }
            }

            await player.AddCharacterSkillAsync(new CharacterSkillDto
            {
                Id = Guid.NewGuid(),
                SkillId = skillShop.SkillId,
                Skill = skillShop.Skill,
                CharacterId = player.Id
            });

            player.Character.Gold -= skillShop.Skill.Price;
            await player.ActualizeUiGold();
            await player.ActualizeUiSkillList();
            await player.ActualizeUiQuicklist();
            await player.SendChatMessageAsync(PlayerMessages.SKILL_YOU_LEARNED_SKILL_X, SayColorType.LightGreen);
            await player.ActualizeUiExpBar();
        }

        private async Task HandleNpcItemBuyRequest(IPlayerEntity player, ShopBuyEvent shopBuy, Shop shop)
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
                await player.SendPacketAsync(player.GenerateShopMemoPacket(SMemoPacketType.FailNpc, player.GetLanguage(PlayerMessages.YOU_DONT_HAVE_ENOUGH_GOLD)));
                return;
            }

            if (isReputBuy)
            {
                if (price > player.Character.Reput)
                {
                    await player.SendPacketAsync(player.GenerateShopMemoPacket(SMemoPacketType.FailNpc, player.GetLanguage(PlayerMessages.YOU_DONT_HAVE_ENOUGH_REPUTATION)));
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
                await player.SendPacketAsync(player.GenerateShopMemoPacket(SMemoPacketType.FailNpc, player.GetLanguage(PlayerMessages.YOU_DONT_HAVE_ENOUGH_SPACE_IN_INVENTORY)));
                return;
            }

            // add item to inventory
            var itemFactory = ChickenContainer.Instance.Resolve<IItemInstanceDtoFactory>();
            ItemInstanceDto newitem = itemFactory.CreateItem(item.ItemId, amount, (sbyte)rare);

            if (isReputBuy)
            {
                player.Character.Reput -= price;
                await player.ActualizeUiReputation();
                // player.SendPacket "reput decreased"
            }
            else
            {
                player.Character.Gold -= (long)(price * percent);
                await player.ActualizeUiGold();
            }

            await player.EmitEventAsync(new InventoryAddItemEvent
            {
                ItemInstance = newitem
            });
        }

        private static Task HandlePlayerShopBuyRequest(IPlayerEntity player, ShopBuyEvent shopBuy, IPlayerEntity shop)
        {
            // todo
            return Task.CompletedTask;
        }
    }
}