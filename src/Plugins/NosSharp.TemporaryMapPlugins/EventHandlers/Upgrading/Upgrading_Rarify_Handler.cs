using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Maths;
using ChickenAPI.Data.Enums.Game.Items;
using ChickenAPI.Data.Enums.Game.Items.Upgrade;
using ChickenAPI.Data.Item;
using ChickenAPI.Game;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Inventory.ItemUpgrade.Extension;
using ChickenAPI.Game.Shops.Extensions;
using ChickenAPI.Packets.Enumerations;
using EquipmentType = ChickenAPI.Data.Enums.Game.Items.EquipmentType;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Upgrading_Rarify_Handler : GenericEventPostProcessorBase<RarifyEvent>
    {
        private readonly IGameConfiguration _configuration;
        private readonly IRandomGenerator _randomGenerator;

        public Upgrading_Rarify_Handler(IGameConfiguration configuration, IRandomGenerator randomGenerator, ILogger log) : base(log)
        {
            _configuration = configuration;
            _randomGenerator = randomGenerator;
        }

        protected override async Task Handle(RarifyEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            double rnd;

            double[] rare =
            {
                _configuration.RarifyChances.Raren2, _configuration.RarifyChances.Raren1, _configuration.RarifyChances.Rare0,
                _configuration.RarifyChances.Rare1, _configuration.RarifyChances.Rare2, _configuration.RarifyChances.Rare3,
                _configuration.RarifyChances.Rare4, _configuration.RarifyChances.Rare5, _configuration.RarifyChances.Rare6,
                _configuration.RarifyChances.Rare7, _configuration.RarifyChances.Rare8
            };
            sbyte[] rareitem = { -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            if (e.Mode != RarifyMode.Drop || e.Item.Item.ItemType == ItemType.Shell)
            {
                rare[0] = 0;
                rare[1] = 0;
                rare[2] = 0;
                rnd = _randomGenerator.Next(0, 80);
            }
            else
            {
                rnd = _randomGenerator.Next(0, 1000) / 10D;
            }

            if (e.Protection == RarifyProtection.RedAmulet ||
                e.Protection == RarifyProtection.HeroicAmulet ||
                e.Protection == RarifyProtection.RandomHeroicAmulet)
            {
                for (byte i = 0; i < rare.Length; i++)
                {
                    rare[i] = (byte)(rare[i] * _configuration.RarifyChances.ReducedChanceFactor);
                }
            }

            ItemInstanceDto amulet = player.Inventory.GetWeared(EquipmentType.Amulet);
            switch (e.Mode)
            {
                case RarifyMode.Free:
                    break;

                case RarifyMode.Reduce:
                    if (amulet == null)
                    {
                        return;
                    }

                    if (e.Item.Rarity < 8 || !e.Item.Item.IsHeroic)
                    {
                        await player.SendChatMessageAsync("NOT_MAX_RARITY", SayColorType.Yellow);
                        return;
                    }

                    e.Item.Rarity -= (sbyte)_randomGenerator.Next(0, 7);
                    e.Item.SetRarityPoint();
                    e.Item.GenerateHeroicShell(e.Protection);

                    // session.Character.DeleteItemByItemInstanceId(amulet.Id);
                    // await session.SendPacketAsync(session.Character.GenerateEquipment());
                    await player.SendPacketAsync(player.GenerateInfoBubble("AMULET_DESTROYED"));
                    await player.NotifyRarifyResult(e.Item.Rarity);
                    await player.SendPacketAsync(e.Item?.GenerateIvnPacket());
                    return;

                case RarifyMode.Success:
                    if (e.Item.Item.IsHeroic && e.Item.Rarity >= 8 || !e.Item.Item.IsHeroic && e.Item.Rarity <= 7)
                    {
                        await player.SendChatMessageAsync("ALREADY_MAX_RARE", SayColorType.Yellow);
                        return;
                    }

                    e.Item.Rarity += 1;
                    e.Item.SetRarityPoint();
                    if (e.Item.Item.IsHeroic)
                    {
                        e.Item.GenerateHeroicShell(RarifyProtection.RandomHeroicAmulet);
                    }

                    ItemInstanceDto inventory = e.Item;
                    if (inventory != null)
                    {
                        await player.SendPacketAsync(e.Item?.GenerateIvnPacket());
                    }

                    return;

                case RarifyMode.Normal:

                    // TODO: Normal Item Amount
                    if (player.Character.Gold < _configuration.RarifyChances.GoldPrice)
                    {
                        return;
                    }

                    if (player.Inventory.GetItemQuantityById(_configuration.RarifyChances.RarifyItemNeededVnum) < _configuration.RarifyChances.RarifyItemNeededQuantity)
                    {
                        // not enough quantity !
                        return;
                    }

                    if (e.Protection == RarifyProtection.Scroll && !e.IsCommand && !player.Inventory.HasItem(_configuration.RarifyChances.ScrollVnum))
                    {
                        return;
                    }

                    if ((e.Protection == RarifyProtection.Scroll || e.Protection == RarifyProtection.BlueAmulet ||
                        e.Protection == RarifyProtection.RedAmulet) && !e.IsCommand && e.Item.Item.IsHeroic)
                    {
                        await player.SendTopscreenMessage("ITEM_IS_HEROIC", MessageType.Whisper);
                        return;
                    }

                    if ((e.Protection == RarifyProtection.HeroicAmulet ||
                        e.Protection == RarifyProtection.RandomHeroicAmulet) && !e.Item.Item.IsHeroic)
                    {
                        await player.SendTopscreenMessage("ITEM_NOT_HEROIC", MessageType.Whisper);
                        return;
                    }

                    if (e.Item.Item.IsHeroic && e.Item.Rarity == 8)
                    {
                        await player.SendTopscreenMessage("ALREADY_MAX_RARE", MessageType.Whisper);
                        return;
                    }

                    if (e.Protection == RarifyProtection.Scroll && !e.IsCommand)
                    {
                        // remove item ScrollVnum
                        await player.SendPacketAsync(player.GenerateShopEndPacket(player.Inventory.HasItem(_configuration.RarifyChances.ScrollVnum)
                            ? ShopEndPacketType.CloseSubWindow
                            : ShopEndPacketType.CloseWindow));
                    }

                    await player.GoldLess(_configuration.RarifyChances.GoldPrice);
                    /*session.Character.Inventory.RemoveItemAmount(cellaVnum, cella);*/
                    break;

                case RarifyMode.Drop:
                    break;

                default:
                    return;
            }

            if (e.Item.Item.IsHeroic && e.Protection != RarifyProtection.None)
            {
                if (rnd < rare[10])
                {
                    if (e.Mode != RarifyMode.Drop)
                    {
                        await player.NotifyRarifyResult(8);
                    }

                    e.Item.Rarity = 8;

                    ItemInstanceDto inventory = e.Item;
                    if (inventory != null)
                    {
                        await player.SendPacketAsync(inventory.GenerateIvnPacket());
                    }

                    e.Item.GenerateHeroicShell(e.Protection);
                    e.Item.SetRarityPoint();
                    return;
                }
            }

            bool sayfail = false;
            for (byte y = 9; y != 0; y--)
            {
                if (!(rnd < rare[y]) || e.Protection == RarifyProtection.Scroll && e.Item.Rarity >= rareitem[y])
                {
                    continue;
                }

                await e.RarifyAsync(player, rareitem[y]);
                sayfail = true;
                break;
            }

            if (!sayfail)
            {
                await e.FailAsync(player);
            }

            if (e.Mode == RarifyMode.Drop)
            {
                return;
            }

            ItemInstanceDto inventoryb = e.Item;
            if (inventoryb != null)
            {
                await player.SendPacketAsync(inventoryb.GenerateIvnPacket());
            }
        }
    }
}