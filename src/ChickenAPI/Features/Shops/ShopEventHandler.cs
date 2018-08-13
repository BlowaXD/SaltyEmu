using System;
using System.Linq;
using System.Text;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using ChickenAPI.Game.Data.TransferObjects.Skills;
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
            throw new NotImplementedException();
        }

        private static void HandleSellRequest(IPlayerEntity player, SellShopEventArgs sell)
        {
            throw new NotImplementedException();
        }
    }
}