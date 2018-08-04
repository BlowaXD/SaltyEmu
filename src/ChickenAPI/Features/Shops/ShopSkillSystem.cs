using System.Linq;
using System.Text;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Shops.Args;
using ChickenAPI.Game.Features.Shops.Packets;

namespace ChickenAPI.Game.Features.Shops
{
    public class ShopSystem : NotifiableSystemBase
    {
        public ShopSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            switch (e)
            {
                case GetShopInformationEventArgs getinfos:
                    SendInformations(getinfos, entity);
                    break;
                case BuyShopEventArgs buy:
                    HandleBuyRequest(entity, buy);
                    break;
                case SellShopEventArgs sell:
                    HandleSellRequest(entity, sell);
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

            int typeshop = 100;

            var tmp = new StringBuilder();
            float percent = 1.0f;
            foreach (ShopItemDto itemInfo in getinfos.Shop.Items.Where(s => s.Type == getinfos.Packet.Type))
            {
                tmp.Append(' ');
                float price = itemInfo.Item.ReputPrice > 0 ? itemInfo.Item.ReputPrice : itemInfo.Item.Price * percent;
                byte color = itemInfo.Color != 0 ? itemInfo.Item.Color : itemInfo.Item.BasicUpgrade;
                int rare = itemInfo.Type == 0 ? itemInfo.Rare : -1;
                if (itemInfo.Type != 0 || itemInfo.Item.ReputPrice <= 0)
                {
                    continue;
                }

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

            foreach (ShopSkillDto skill in getinfos.Shop.Skills.Where(s => s.Type.Equals(getinfos.Packet.Type)))
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

            player.SendPacket(new NInvPacket()
            {
                ShopList = tmp.ToString(),
                ShopType = typeshop,
                Unknown = 0,
                VisualId = getinfos.Shop.MapNpcId,
                VisualType = 2
            });
        }

        private static void HandleBuyRequest(IEntity entity, BuyShopEventArgs buy)
        {
            throw new System.NotImplementedException();
        }

        private static void HandleSellRequest(IEntity entity, SellShopEventArgs sell)
        {
            throw new System.NotImplementedException();
        }
    }
}