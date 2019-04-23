using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChickenAPI.Data.Shop;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Shop;

namespace ChickenAPI.Game.Shops.Extensions
{
    public static class NInvPacketExtensions
    {
        private static List<NInvItemSubPacket> GetShopList(IEnumerable<PersonalShopItem> items)
        {
            var tmp = new List<NInvItemSubPacket>();

            foreach (PersonalShopItem itemInfo in items)
            {
                // tmp.Append(' ');
                long upgradeOrPrice = itemInfo.ItemInstance.Item.Type == PocketType.Equipment ? itemInfo.ItemInstance.Upgrade : itemInfo.Price;
                int rareOrQuantity = itemInfo.ItemInstance.Item.Type == PocketType.Equipment ? itemInfo.ItemInstance.Rarity : itemInfo.Quantity;
                double priceOrMinus = itemInfo.ItemInstance.Item.Type == PocketType.Equipment ? itemInfo.Price : -1;

                tmp.Add(new NInvItemSubPacket
                {
                    Price = upgradeOrPrice,
                    UpgradeDesign = rareOrQuantity,
                    Slot = (byte)itemInfo.Slot,
                    VNum = (short)itemInfo.ItemInstance.ItemId,
                });

                // tmp.Append($"{(byte)itemInfo.ItemInstance.Item.Type}.{itemInfo.Slot}.{itemInfo.ItemInstance.ItemId}.{rareOrQuantity}.{upgradeOrPrice}.{priceOrMinus}");
            }

            return tmp;
        }

        private static List<NInvItemSubPacket> GetShopList(IEnumerable<ShopItemDto> items, double percent)
        {
            var tmp = new List<NInvItemSubPacket>();

            foreach (ShopItemDto itemInfo in items)
            {
                double price = itemInfo.Item.ReputPrice > 0 ? itemInfo.Item.ReputPrice : itemInfo.Item.Price * percent;
                byte color = itemInfo.Color != 0 ? itemInfo.Item.Color : itemInfo.Item.BasicUpgrade;
                int rareOrQuantity = itemInfo.Item.Type != PocketType.Equipment ? -1 : itemInfo.Rare;


            }

            return tmp;
        }


        public static NInvPacket GenerateNInvPacket(this IPlayerEntity player, PersonalShop shop) =>
            new NInvPacket
            {
                VisualType = shop.Owner.Type,
                VisualId = shop.Owner.Id,
                ShopKind = 0,
                Unknown = 0,
                Items = GetShopList(shop.ShopItems),
                Skills = null,
            };

        public static NInvPacket GenerateNInvPacket(this IPlayerEntity player, Shop shop, byte type)
        {
            byte typeshop = 0;
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


            if (typeshop == 0 && !shop.Skills.Any())
            {
                typeshop = 100;
            }

            List<short> skillIds = new List<short>();

            foreach (ShopSkillDto skill in shop.Skills.Where(s => s.Type == type))
            {
                if (skill.Type != 0)
                {
                    typeshop = 1;
                    if (skill.Skill.Class == (byte)player.Character.Class)
                    {
                        skillIds.Add((short)skill.SkillId);
                    }
                }
                else
                {
                    skillIds.Add((short)skill.SkillId);
                }
            }


            return new NInvPacket
            {
                VisualType = shop.Owner.Type,
                VisualId = shop.Owner.Id,
                ShopKind = typeshop,
                Unknown = 0,
                Items = GetShopList(shop.Items.Where(s => s.Type == type), percent),
                Skills = skillIds
            };
        }
    }
}