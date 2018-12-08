using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers;
using ChickenAPI.Packets.Game.Server.Shop;
using System;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Handler
{
    public class SumItem
    {
        private static readonly IGameConfiguration Configuration = new Lazy<IGameConfiguration>(ChickenContainer.Instance.Resolve<IGameConfiguration>).Value;

        [ItemUpgradeHandler(UpgradePacketType.SumResistance)]
        public void SumResistance(IPlayerEntity player, ItemUpgradeEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }

            if (e.SecondItem == null)
            {
                return;
            }

            if (e.Item.Sum + e.SecondItem.Sum >= 6 || (e.SecondItem.Item.EquipmentSlot != EquipmentType.Gloves ||
                e.Item.Item.EquipmentSlot != EquipmentType.Gloves) &&
               (e.Item.Item.EquipmentSlot != EquipmentType.Boots || e.SecondItem.Item.EquipmentSlot != EquipmentType.Boots))
            {
                return;
            }

            if (player.Character.Gold < Configuration.Summing.GoldPrice[e.Item.Sum + e.SecondItem.Sum])
            {
                return;
            }

            if (player.Inventory.GetItemQuantityById(Configuration.Summing.SandVnum) < Configuration.Summing.SandAmount[e.Item.Sum + e.SecondItem.Sum])
            {
                return;
            }

            player.EmitEvent(new SummingEventArgs
            {
                Item = e.Item,
                SecondItem = e.SecondItem
            });
        }
    }
}
