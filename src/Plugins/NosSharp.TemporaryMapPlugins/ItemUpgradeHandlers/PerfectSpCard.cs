using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers;
using ChickenAPI.Game.Inventory.ItemUpgrade.Handlers.Handling;

namespace SaltyEmu.BasicPlugin.ItemUpgradeHandlers
{
    public class PerfectSpCard
    {
        private static readonly IGameConfiguration Configuration = new Lazy<IGameConfiguration>(ChickenContainer.Instance.Resolve<IGameConfiguration>).Value;

        [ItemUpgradeHandler(UpgradePacketType.PerfectSp)]
        public static void PerfectSpCardNPC(IPlayerEntity player, ItemUpgradeEvent e)
        {
            if (e.Item.Rarity == -2)
            {
                player.SendChatMessageAsync("CANT_UPGRADE_DESTROYED_SP", SayColorType.Yellow);
                return;
            }

            if (e.Item.Item.EquipmentSlot != EquipmentType.Sp)
            {
                return;
            }

            short stonevnum;
            byte upmode = 1;

            switch (e.Item.Item.Morph)
            {
                case 2:
                    stonevnum = 2514;
                    break;

                case 6:
                    stonevnum = 2514;
                    break;

                case 9:
                    stonevnum = 2514;
                    break;

                case 12:
                    stonevnum = 2514;
                    break;

                case 3:
                    stonevnum = 2515;
                    break;

                case 4:
                    stonevnum = 2515;
                    break;

                case 14:
                    stonevnum = 2515;
                    break;

                case 5:
                    stonevnum = 2516;
                    break;

                case 11:
                    stonevnum = 2516;
                    break;

                case 15:
                    stonevnum = 2516;
                    break;

                case 10:
                    stonevnum = 2517;
                    break;

                case 13:
                    stonevnum = 2517;
                    break;

                case 7:
                    stonevnum = 2517;
                    break;

                case 17:
                    stonevnum = 2518;
                    break;

                case 18:
                    stonevnum = 2518;
                    break;

                case 19:
                    stonevnum = 2518;
                    break;

                case 20:
                    stonevnum = 2519;
                    break;

                case 21:
                    stonevnum = 2519;
                    break;

                case 22:
                    stonevnum = 2519;
                    break;

                case 23:
                    stonevnum = 2520;
                    break;

                case 24:
                    stonevnum = 2520;
                    break;

                case 25:
                    stonevnum = 2520;
                    break;

                case 26:
                    stonevnum = 2521;
                    break;

                case 27:
                    stonevnum = 2521;
                    break;

                case 28:
                    stonevnum = 2521;
                    break;

                default:
                    return;
            }

            if (e.Item.SpecialistUpgrade > 99)
            {
                return;
            }

            if (e.Item.SpStoneUpgrade > 80)
            {
                upmode = 5;
            }
            else if (e.Item.SpStoneUpgrade > 60)
            {
                upmode = 4;
            }
            else if (e.Item.SpStoneUpgrade > 40)
            {
                upmode = 3;
            }
            else if (e.Item.SpStoneUpgrade > 20)
            {
                upmode = 2;
            }

            if (e.Item.IsFixed)
            {
                return;
            }

            if (player.Character.Gold < Configuration.PerfectSp.GoldPrice[upmode - 1])
            {
                return;
            }

            if (player.Inventory.GetItemQuantityById(stonevnum) < Configuration.PerfectSp.GoldPrice[upmode - 1])
            {
                return;
            }

            player.EmitEvent(new PerfectSPCardEvent
            {
                SpCard = e.Item,
                StoneVnum = stonevnum,
                UpMode = upmode
            });
        }
    }
}
