using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChickenAPI.Data.Shop;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Packets.Game.Client.Shops;

namespace ChickenAPI.Game.Shops.Extensions
{
    public static class NInvPacketExtensions
    {
        private static string GetShopList(IEnumerable<PersonalShopItem> items)
        {
            var tmp = new StringBuilder();

            foreach (PersonalShopItem itemInfo in items)
            {
                tmp.Append(' ');
                long upgradeOrPrice = itemInfo.ItemInstance.Item.Type == InventoryType.Equipment ? itemInfo.ItemInstance.Upgrade : itemInfo.Price;
                int rareOrQuantity = itemInfo.ItemInstance.Item.Type == InventoryType.Equipment ? itemInfo.ItemInstance.Rarity : itemInfo.Quantity;
                double priceOrMinus = itemInfo.ItemInstance.Item.Type == InventoryType.Equipment ? itemInfo.Price : -1;

                tmp.Append($"{(byte)itemInfo.ItemInstance.Item.Type}.{itemInfo.Slot}.{itemInfo.ItemInstance.ItemId}.{rareOrQuantity}.{upgradeOrPrice}.{priceOrMinus}");
            }

            return tmp.ToString();
        }

        private static string GetShopList(IEnumerable<ShopItemDto> items, double percent)
        {
            var tmp = new StringBuilder();

            foreach (ShopItemDto itemInfo in items)
            {
                tmp.Append(' ');
                double price = itemInfo.Item.ReputPrice > 0 ? itemInfo.Item.ReputPrice : itemInfo.Item.Price * percent;
                byte color = itemInfo.Color != 0 ? itemInfo.Item.Color : itemInfo.Item.BasicUpgrade;
                int rareOrQuantity = itemInfo.Item.Type != InventoryType.Equipment ? -1 : itemInfo.Rare;

                tmp.Append($"{(byte)itemInfo.Item.Type}.{itemInfo.Slot}.{itemInfo.ItemId}.{rareOrQuantity}.");
                if (itemInfo.Item.Type == InventoryType.Equipment)
                {
                    tmp.Append(color);
                    tmp.Append('.');
                }

                tmp.Append(price);
            }

            return tmp.ToString();
        }


        public static NInvPacket GenerateNInvPacket(this IPlayerEntity player, PersonalShop shop) =>
            new NInvPacket
            {
                VisualType = shop.Owner.Type,
                VisualId = shop.Owner.Id,
                ShopType = 0,
                Unknown = 0,
                ShopList = GetShopList(shop.ShopItems),
                ShopSkills = new List<long>()
            };

        public static NInvPacket GenerateNInvPacket(this IPlayerEntity player, Shop shop, byte type)
        {
            int typeshop = 0;
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

            List<long> skillIds = new List<long>();

            foreach (ShopSkillDto skill in shop.Skills.Where(s => s.Type == type))
            {
                if (skill.Type != 0)
                {
                    typeshop = 1;
                    if (skill.Skill.Class == (byte)player.Character.Class)
                    {
                        skillIds.Add(skill.SkillId);
                    }
                }
                else
                {
                    skillIds.Add(skill.SkillId);
                }
            }


            return new NInvPacket
            {
                VisualType = shop.Owner.Type,
                VisualId = shop.Owner.Id,
                ShopType = typeshop,
                Unknown = 0,
                ShopList = GetShopList(shop.Items.Where(s => s.Type == type), percent),
                ShopSkills = skillIds
            };
        }
    }
}