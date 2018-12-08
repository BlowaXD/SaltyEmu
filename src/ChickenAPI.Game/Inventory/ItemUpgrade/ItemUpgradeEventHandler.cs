using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Core.Maths;
using ChickenAPI.Game.Player.Extension;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Inventory.ItemUpgrade.Extension;
using ChickenAPI.Game.Shops.Extensions;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Packets.Game.Client.Inventory;
using System.Linq;

namespace ChickenAPI.Game.Inventory.ItemUpgrade
{
    public class ItemUpgradeEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<ItemUpgradeEventHandler>();
        private static readonly IItemUpgradeHandler ItemUpgradeHandler = new Lazy<IItemUpgradeHandler>(() => ChickenContainer.Instance.Resolve<IItemUpgradeHandler>()).Value;
        private static IRandomGenerator Random => new Lazy<IRandomGenerator>(() => ChickenContainer.Instance.Resolve<IRandomGenerator>()).Value;
        private static readonly IGameConfiguration Configuration = new Lazy<IGameConfiguration>(ChickenContainer.Instance.Resolve<IGameConfiguration>).Value;

        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(ItemUpgradeEventArgs),
            typeof(RarifyEventArgs),
            typeof(UpgradeEventArgs),
            typeof(SummingEventArgs),
            typeof(PerfectSPCardEvent),
            typeof(CellonItemEventArgs)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case CellonItemEventArgs cellonevent:
                    Cellon(entity as IPlayerEntity, cellonevent);
                    break;

                case PerfectSPCardEvent perfectevent:
                    Perfect(entity as IPlayerEntity, perfectevent);
                    break;

                case SummingEventArgs sumevent:
                    SummingItem(entity as IPlayerEntity, sumevent);
                    break;

                case ItemUpgradeEventArgs upgradeevent:
                    ItemUpgradeHandler.Execute(entity as IPlayerEntity, upgradeevent);
                    break;

                case RarifyEventArgs rarifyEvent:
                    RarifyItem(entity as IPlayerEntity, rarifyEvent);
                    break;

                case UpgradeEventArgs upgradeEvent:
                    switch (upgradeEvent.Type)
                    {
                        case UpgradeTypeEvent.Item:
                            UpgradeItem(entity as IPlayerEntity, upgradeEvent);
                            break;

                        case UpgradeTypeEvent.Specialist:
                            UpgradeSpecialist(entity as IPlayerEntity, upgradeEvent);
                            break;
                    }
                    break;
            }
        }

        public void Cellon(IPlayerEntity player, CellonItemEventArgs e)
        {
            player.GoldLess(e.GoldAmount);
            //Session.Character.Inventory.RemoveItemAmount(cellon.ItemVNum);

            // GENERATE OPTION
            EquipmentOptionDto option = CellonGeneratorHelper.Instance.GenerateOption(e.Cellon.Item.EffectValue);

            // FAIL
            if (option == null || e.Jewelry.EquipmentOptions.Any(s => s.Type == option.Type))
            {
                player.SendTopscreenMessage("CELLONING_FAILED", MsgPacketType.White);
                player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow);
                return;
            }

            // SUCCESS
            e.Jewelry.EquipmentOptions.Add(option);
            player.SendTopscreenMessage("CELLONING_SUCCESS", MsgPacketType.White);
            player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow);
        }

        public void Perfect(IPlayerEntity player, PerfectSPCardEvent e)
        {
            double rnd = Random.Next();
            if (rnd < Configuration.PerfectSp.UpSuccess[e.UpMode - 1])
            {
                byte type = (byte)Random.Next(0, 16), count = 1;
                if (e.UpMode == 4)
                {
                    count = 2;
                }

                if (e.UpMode == 5)
                {
                    count = (byte)Random.Next(3, 6);
                }

                player.SendPacket(player.GenerateEffectPacket(3005));

                int stoneup = (type < 3 ? e.SpCard.SpDamage : type < 6 ? e.SpCard.SpDefence : type < 9 ? e.SpCard.SpElement : type < 12 ?
                    e.SpCard.SpHP : type == 12 ? e.SpCard.SpFire : type == 13 ? e.SpCard.SpWater : type == 14 ? e.SpCard.SpLight : e.SpCard.SpDark);

                stoneup += count;
                player.SendTopscreenMessage("PERFECTSP_SUCCESS", MsgPacketType.White);
                player.SendChatMessage("PERFECTSP_SUCCESS", SayColorType.Green);


                e.SpCard.SpStoneUpgrade++;
            }
            else
            {
                player.SendTopscreenMessage("PERFECTSP_FAILURE", MsgPacketType.White);
                player.SendChatMessage("PERFECTSP_FAILURE", SayColorType.Purple);
            }

            player.SendPacket(e.SpCard.GenerateIvnPacket());
            player.GoldLess(Configuration.PerfectSp.GoldPrice[e.UpMode - 1]);
            //CharacterSession.Character.Inventory.RemoveItemAmount(stonevnum, stoneprice[upmode - 1]);
            player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow);
        }

        public void SummingItem(IPlayerEntity player, SummingEventArgs e)
        {
            //session.Character.Inventory.RemoveItemAmount(sandVnum, (byte)sand[Upgrade + itemToSum.Upgrade]);
            player.GoldLess(Configuration.Summing.GoldPrice[e.Item.Sum + e.SecondItem.Sum]);

            double rnd = Random.Next();
            if (rnd < Configuration.Summing.UpSucess[e.Item.Sum + e.SecondItem.Sum])
            {
                e.Item.Sum += (byte)(e.SecondItem.Sum + 1);
                e.Item.DarkResistance += (short)(e.SecondItem.DarkResistance + e.SecondItem.Item.DarkResistance);
                e.Item.LightResistance += (short)(e.SecondItem.LightResistance + e.SecondItem.Item.LightResistance);
                e.Item.WaterResistance += (short)(e.SecondItem.WaterResistance + e.SecondItem.Item.WaterResistance);
                e.Item.FireResistance += (short)(e.SecondItem.FireResistance + e.SecondItem.Item.FireResistance);
                //session.Character.DeleteItemByItemInstanceId(itemToSum.Id);
                player.SendPacket(new PdtiPacket { Unknow = 10, Unknow2 = 1, Unknow3 = 27, Unknow4 = 0, ItemVnum = e.Item.Item.Vnum, ItemUpgrade = e.Item.Sum });
                player.SendChatMessage("SUM_SUCCESS", SayColorType.Green);
                player.SendTopscreenMessage("SUM_SUCCESS", MsgPacketType.Whisper);
                player.SendGuri(19, 1, 1324);
                player.SendPacket(e.Item?.GenerateIvnPacket());
            }
            else
            {
                player.SendChatMessage("SUM_FAILED", SayColorType.Purple);
                player.SendTopscreenMessage("SUM_FAILED", MsgPacketType.Whisper);
                player.SendGuri(19, 1, 1332);
                //session.Character.DeleteItemByItemInstanceId(itemToSum.Id);
                //session.Character.DeleteItemByItemInstanceId(Id);
            }

            player.Broadcast(player.GenerateGuriPacket(6, 1));
            player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow);
        }

        public void RarifyItem(IPlayerEntity player, RarifyEventArgs e)
        {
            double rnd;

            double[] rare =
            {
                Configuration.RarifyChances.Raren2, Configuration.RarifyChances.Raren1, Configuration.RarifyChances.Rare0,
                Configuration.RarifyChances.Rare1, Configuration.RarifyChances.Rare2, Configuration.RarifyChances.Rare3,
                Configuration.RarifyChances.Rare4, Configuration.RarifyChances.Rare5, Configuration.RarifyChances.Rare6,
                Configuration.RarifyChances.Rare7, Configuration.RarifyChances.Rare8
            };
            sbyte[] rareitem = { -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            if (e.Mode != RarifyMode.Drop || e.Item.Item.ItemType == ItemType.Shell)
            {
                rare[0] = 0;
                rare[1] = 0;
                rare[2] = 0;
                rnd = Random.Next(0, 80);
            }
            else
            {
                rnd = Random.Next(0, 1000) / 10D;
            }

            if (e.Protection == RarifyProtection.RedAmulet ||
                e.Protection == RarifyProtection.HeroicAmulet ||
                e.Protection == RarifyProtection.RandomHeroicAmulet)
            {
                for (byte i = 0; i < rare.Length; i++)
                {
                    rare[i] = (byte)(rare[i] * Configuration.RarifyChances.ReducedChanceFactor);
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
                        player.SendChatMessage("NOT_MAX_RARITY", SayColorType.Yellow);
                        return;
                    }

                    e.Item.Rarity -= (sbyte)Random.Next(0, 7);
                    e.Item.SetRarityPoint();
                    e.Item.GenerateHeroicShell(e.Protection);

                    // session.Character.DeleteItemByItemInstanceId(amulet.Id);
                    // session.SendPacket(session.Character.GenerateEquipment());
                    player.SendPacket(player.GenerateInfoBubble("AMULET_DESTROYED"));
                    player.NotifyRarifyResult(e.Item.Rarity);
                    player.SendPacket(e.Item?.GenerateIvnPacket());
                    return;

                case RarifyMode.Success:
                    if (e.Item.Item.IsHeroic && e.Item.Rarity >= 8 || !e.Item.Item.IsHeroic && e.Item.Rarity <= 7)
                    {
                        player.SendChatMessage("ALREADY_MAX_RARE", SayColorType.Yellow);
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
                        player.SendPacket(e.Item?.GenerateIvnPacket());
                    }

                    return;

                case RarifyMode.Normal:

                    // TODO: Normal Item Amount
                    if (player.Character.Gold < Configuration.RarifyChances.GoldPrice)
                    {
                        return;
                    }

                    if (player.Inventory.GetItemQuantityById(Configuration.RarifyChances.RarifyItemNeededVnum) < Configuration.RarifyChances.RarifyItemNeededQuantity)
                    {
                        // not enough quantity !
                        return;
                    }


                    if (e.Protection == RarifyProtection.Scroll && !e.IsCommand && !player.Inventory.HasItem(Configuration.RarifyChances.ScrollVnum))
                    {
                        return;
                    }

                    if ((e.Protection == RarifyProtection.Scroll || e.Protection == RarifyProtection.BlueAmulet ||
                        e.Protection == RarifyProtection.RedAmulet) && !e.IsCommand && e.Item.Item.IsHeroic)
                    {
                        player.SendTopscreenMessage("ITEM_IS_HEROIC", MsgPacketType.Whisper);
                        return;
                    }

                    if ((e.Protection == RarifyProtection.HeroicAmulet ||
                        e.Protection == RarifyProtection.RandomHeroicAmulet) && !e.Item.Item.IsHeroic)
                    {
                        player.SendTopscreenMessage("ITEM_NOT_HEROIC", MsgPacketType.Whisper);
                        return;
                    }

                    if (e.Item.Item.IsHeroic && e.Item.Rarity == 8)
                    {
                        player.SendTopscreenMessage("ALREADY_MAX_RARE", MsgPacketType.Whisper);
                        return;
                    }

                    if (e.Protection == RarifyProtection.Scroll && !e.IsCommand)
                    {
                        // remove item ScrollVnum
                        player.SendPacket(player.GenerateShopEndPacket(player.Inventory.HasItem(Configuration.RarifyChances.ScrollVnum)
                            ? ShopEndPacketType.CloseSubWindow
                            : ShopEndPacketType.CloseWindow));
                    }

                    player.GoldLess(Configuration.RarifyChances.GoldPrice);
                    /*session.Character.Inventory.RemoveItemAmount(cellaVnum, cella);*/
                    break;

                case RarifyMode.Drop:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(e.Mode), e.Mode, null);
            }

            if (e.Item.Item.IsHeroic && e.Protection != RarifyProtection.None)
            {
                if (rnd < rare[10])
                {
                    if (e.Mode != RarifyMode.Drop)
                    {
                        player.NotifyRarifyResult(8);
                    }

                    e.Item.Rarity = 8;

                    ItemInstanceDto inventory = e.Item;
                    if (inventory != null)
                    {
                        player.SendPacket(inventory.GenerateIvnPacket());
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

                e.Rarify(player, rareitem[y]);
                sayfail = true;
                break;
            }

            if (!sayfail)
            {
                e.Fails(player);
            }

            if (e.Mode == RarifyMode.Drop)
            {
                return;
            }

            ItemInstanceDto inventoryb = e.Item;
            if (inventoryb != null)
            {
                player.SendPacket(inventoryb.GenerateIvnPacket());
            }
        }

        public void UpgradeItem(IPlayerEntity player, UpgradeEventArgs e)
        {
            short[] upfail = e.Item.Rarity >= 8 ? Configuration.UpgradeItem.UpFailR8 : Configuration.UpgradeItem.UpFail;
            short[] upfix = e.Item.Rarity >= 8 ? Configuration.UpgradeItem.UpFixR8 : Configuration.UpgradeItem.UpFix;
            int[] goldprice = e.Item.Rarity >= 8 ? Configuration.UpgradeItem.GoldPriceR8 : Configuration.UpgradeItem.GoldPrice;
            short[] cella = e.Item.Rarity >= 8 ? Configuration.UpgradeItem.CellaAmountR8 : Configuration.UpgradeItem.CellaAmount;
            short[] gem = e.Item.Rarity >= 8 ? Configuration.UpgradeItem.GemAmountR8 : Configuration.UpgradeItem.GemAmount;

            if (e.HasAmulet == FixedUpMode.HasAmulet && e.Item.IsFixed)
            {
                upfix = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }

            if (e.Item.IsFixed && e.HasAmulet == FixedUpMode.None)
            {
                player.SendChatMessage("ITEM_IS_FIXED", SayColorType.Yellow);
                player.GenerateShopEndPacket(ShopEndPacketType.CloseWindow);
                return;
            }

            if (e.Item.IsFixed && e.HasAmulet == FixedUpMode.HasAmulet)
            {
                e.Item.IsFixed = !e.Item.IsFixed;
            }

            switch (e.Mode)
            {
                case UpgradeMode.Free:
                    break;

                case UpgradeMode.Reduced:

                    // TODO: Reduced Item Amount
                    if (player.Character.Gold < (long)(goldprice[e.Item.Upgrade] * Configuration.UpgradeItem.ReducedPriceFactor))
                    {
                        player.SendChatMessage("NOT_ENOUGH_MONEY", SayColorType.Yellow);
                        return;
                    }

                    if (player.Inventory.GetItemQuantityById(Configuration.UpgradeItem.CellaVnum) < cella[e.Item.Upgrade] * Configuration.UpgradeItem.ReducedPriceFactor)
                    {
                        player.SendChatMessage("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                        return;
                    }

                    if (e.Protection == UpgradeProtection.Protected && !e.IsCommand &&
                        player.Inventory.GetItemQuantityById(Configuration.UpgradeItem.GoldScrollVnum) < 1)
                    {
                        player.SendChatMessage("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                        return;
                    }

                    if (player.Inventory.GetItemQuantityById(e.Item.Upgrade < 5 ? Configuration.UpgradeItem.GemVnum : Configuration.UpgradeItem.GemFullVnum) < gem[e.Item.Upgrade])
                    {
                        player.SendChatMessage("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                        return;
                    }

                    // player.inventory.removeitemamount( upgrade < 5 ? gemvnum : gemfullvnum , gem[e.item.upgrade]);

                    if (e.Protection == UpgradeProtection.Protected && !e.IsCommand)
                    {
                        // remove Item gold scroll x 1
                        player.SendPacket(player.GenerateShopEndPacket(player.Inventory.HasItem(Configuration.UpgradeItem.GoldScrollVnum)
                           ? ShopEndPacketType.CloseSubWindow
                           : ShopEndPacketType.CloseWindow));
                    }

                    if (e.HasAmulet == FixedUpMode.HasAmulet && e.Item.IsFixed)
                    {
                        var amulet = player.Inventory.GetWeared(EquipmentType.Amulet);
                        /*amulet.DurabilityPoint -= 1;
                        if (amulet.DurabilityPoint <= 0)
                        {
                            session.Character.DeleteItemByItemInstanceId(amulet.Id);
                            session.SendPacket($"info {Language.Instance.GetMessageFromKey("AMULET_DESTROYED")}");
                            session.SendPacket(session.Character.GenerateEquipment());
                        }*/
                    }

                    player.GoldLess(goldprice[e.Item.Upgrade] * (long)Configuration.UpgradeItem.ReducedPriceFactor);
                    //session.Character.Inventory.RemoveItemAmount(cellaVnum,
                    //    (int)(cella[Upgrade] * reducedpricefactor));
                    break;

                case UpgradeMode.Normal:

                    // TODO: Normal Item Amount

                    if (player.Inventory.GetItemQuantityById(Configuration.UpgradeItem.CellaVnum) < cella[e.Item.Upgrade])
                    {
                        player.SendChatMessage("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                        return;
                    }

                    if (player.Character.Gold < goldprice[e.Item.Upgrade])
                    {
                        player.SendChatMessage("NOT_ENOUGH_MONEY", SayColorType.Yellow);
                        return;
                    }

                    if (e.Protection == UpgradeProtection.Protected && !e.IsCommand &&
                        player.Inventory.GetItemQuantityById(Configuration.UpgradeItem.NormalScrollVnum) < 1)
                    {
                        player.SendChatMessage("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                        return;
                    }


                    if (player.Inventory.GetItemQuantityById(e.Item.Upgrade < 5 ? Configuration.UpgradeItem.GemVnum : Configuration.UpgradeItem.GemFullVnum) < gem[e.Item.Upgrade])
                    {
                        player.SendChatMessage("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                        return;
                    }

                    // player.inventory.removeitemamount( upgrade < 5 ? gemvnum : gemfullvnum , gem[e.item.upgrade]);

                    if (e.Protection == UpgradeProtection.Protected && !e.IsCommand)
                    {
                        // remove Item normal scroll x 1
                        player.SendPacket(player.GenerateShopEndPacket(player.Inventory.HasItem(Configuration.UpgradeItem.NormalScrollVnum)
                           ? ShopEndPacketType.CloseSubWindow
                           : ShopEndPacketType.CloseWindow));
                    }

                    if (e.HasAmulet == FixedUpMode.HasAmulet && e.Item.IsFixed)
                    {
                        var amulet = player.Inventory.GetWeared(EquipmentType.Amulet);
                        /*amulet.DurabilityPoint -= 1;
                        if (amulet.DurabilityPoint <= 0)
                        {
                            session.Character.DeleteItemByItemInstanceId(amulet.Id);
                            session.SendPacket($"info {Language.Instance.GetMessageFromKey("AMULET_DESTROYED")}");
                            session.SendPacket(session.Character.GenerateEquipment());
                        }*/
                    }

                    //session.Character.Inventory.RemoveItemAmount(cellaVnum, cella[Upgrade]);
                    player.GoldLess(goldprice[e.Item.Upgrade]);
                    break;
            }

            ItemInstanceDto wearable = e.Item;

            double rnd = Random.Next();

            if (e.Item.Rarity == 8)
            {
                if (rnd < upfail[e.Item.Upgrade])
                {
                    if (e.Protection == UpgradeProtection.None)
                    {
                        player.SendChatMessage("UPGRADE_FAILED", SayColorType.Purple);
                        player.SendTopscreenMessage("UPGRADE_FAILED", MsgPacketType.Whisper);
                        //session.Character.DeleteItemByItemInstanceId(Id);
                    }
                    else
                    {
                        player.Broadcast(player.GenerateEffectPacket(3004));
                        player.SendChatMessage("SCROLL_PROTECT_USED", SayColorType.Purple);
                        player.SendTopscreenMessage("UPGRADE_FAILED_ITEM_SAVED", MsgPacketType.Whisper);
                    }
                }
                else if (rnd < upfix[e.Item.Upgrade])
                {
                    player.Broadcast(player.GenerateEffectPacket(3004));
                    wearable.IsFixed = true;
                    player.SendChatMessage("UPGRADE_FIXED", SayColorType.Purple);
                    player.SendTopscreenMessage("UPGRADE_FIXED", MsgPacketType.Whisper);
                }
                else
                {
                    player.Broadcast(player.GenerateEffectPacket(3005));
                    player.SendChatMessage("UPGRADE_SUCCESS", SayColorType.Green);
                    player.SendTopscreenMessage("UPGRADE_SUCCESS", MsgPacketType.Whisper);
                    wearable.Upgrade++;
                    if (wearable.Upgrade > 4)
                    {
                        //session.Character.Family?.InsertFamilyLog(FamilyLogType.ItemUpgraded, session.Character.Name,
                        //   itemVNum: wearable.ItemVNum, upgrade: wearable.Upgrade);
                    }

                    player.SendPacket(wearable.GenerateIvnPacket());
                }
            }
            else
            {
                if (rnd < upfix[e.Item.Upgrade])
                {
                    player.Broadcast(player.GenerateEffectPacket(3004));
                    wearable.IsFixed = true;
                    player.SendChatMessage("UPGRADE_FIXED", SayColorType.Purple);
                    player.SendTopscreenMessage("UPGRADE_FIXED", MsgPacketType.Whisper);
                }
                else if (rnd < upfail[e.Item.Upgrade] + upfix[e.Item.Upgrade])
                {
                    if (e.Protection == UpgradeProtection.None)
                    {
                        player.SendChatMessage("UPGRADE_FAILED", SayColorType.Purple);
                        player.SendTopscreenMessage("UPGRADE_FAILED", MsgPacketType.Whisper);
                        //session.Character.DeleteItemByItemInstanceId(Id);
                    }
                    else
                    {
                        player.Broadcast(player.GenerateEffectPacket(3004));
                        player.SendChatMessage("SCROLL_PROTECT_USED", SayColorType.Purple);
                        player.SendTopscreenMessage("UPGRADE_FAILED_ITEM_SAVED", MsgPacketType.Whisper);
                    }
                }
                else
                {
                    player.Broadcast(player.GenerateEffectPacket(3005));
                    player.SendChatMessage("UPGRADE_SUCCESS", SayColorType.Green);
                    player.SendTopscreenMessage("UPGRADE_SUCCESS", MsgPacketType.Whisper);
                    wearable.Upgrade++;
                    if (wearable.Upgrade > 4)
                    {
                        //session.Character.Family?.InsertFamilyLog(FamilyLogType.ItemUpgraded, session.Character.Name,
                        //   itemVNum: wearable.ItemVNum, upgrade: wearable.Upgrade);
                    }

                    player.SendPacket(wearable.GenerateIvnPacket());
                }
            }

            player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow);
        }

        public void UpgradeSpecialist(IPlayerEntity player, UpgradeEventArgs e)
        {
            if (player.Inventory.GetItemQuantityById(Configuration.UpgradeSp.FullmoonVnum) < Configuration.UpgradeSp.FullMoon[e.Item.Upgrade])
            {
                player.SendChatMessage("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                return;
            }

            if (player.Inventory.GetItemQuantityById(Configuration.UpgradeSp.FeatherVnum) < Configuration.UpgradeSp.Feather[e.Item.Upgrade])
            {
                player.SendChatMessage("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                return;
            }

            if (player.Character.Gold < Configuration.UpgradeSp.GoldPrice[e.Item.Upgrade])
            {
                player.SendChatMessage("NOT_ENOUGH_MONEY", SayColorType.Yellow);
                return;
            }

            if (e.Item.Upgrade < 5 ? e.Item.Level < 20 : e.Item.Upgrade < 10 ? e.Item.Level < 40 : e.Item.Level < 50)
            {
                player.SendChatMessage("LVL_REQUIRED", SayColorType.Purple);
                return;
            }

            if (player.Inventory.GetItemQuantityById(e.Item.Upgrade < 5 ?
                (e.Item.Item.Morph <= 16 ? Configuration.UpgradeSp.GreenSoulVnum : Configuration.UpgradeSp.DragonSkinVnum) :
                e.Item.Upgrade < 10 ?
                (e.Item.Item.Morph <= 16 ? Configuration.UpgradeSp.RedScrollVnum : Configuration.UpgradeSp.DragonBloodVnum) :
                (e.Item.Item.Morph <= 16 ? Configuration.UpgradeSp.BlueSoulVnum : Configuration.UpgradeSp.DragonHeartVnum)) <
                Configuration.UpgradeSp.Soul[e.Item.Upgrade])
            {
                player.SendChatMessage("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                return;
            }

            if (e.Protection == UpgradeProtection.Protected)
            {
                if (player.Inventory.GetItemQuantityById(e.Item.Upgrade < 15 ? Configuration.UpgradeSp.BlueScrollVnum
                    : Configuration.UpgradeSp.RedScrollVnum) < 1)
                {
                    player.SendChatMessage("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                    return;
                }
                // remove Item Blue/red scroll x 1
                player.SendPacket(player.GenerateShopEndPacket(player.Inventory.HasItem(e.Item.Upgrade < 15 ? Configuration.UpgradeSp.BlueScrollVnum
                    : Configuration.UpgradeSp.RedScrollVnum)
                   ? ShopEndPacketType.CloseSubWindow
                   : ShopEndPacketType.CloseWindow));
            }

            player.GoldLess(Configuration.UpgradeSp.GoldPrice[e.Item.Upgrade]);
            //CharacterSession.Character.Inventory.RemoveItemAmount(featherVnum, feather[Upgrade]);
            //CharacterSession.Character.Inventory.RemoveItemAmount(fullmoonVnum, fullmoon[Upgrade]);
            ItemInstanceDto wearable = e.Item;
            double rnd = Random.Next();

            if (rnd < Configuration.UpgradeSp.Destroy[e.Item.Upgrade])
            {
                if (e.Protection == UpgradeProtection.Protected)
                {
                    player.SendPacket(player.GenerateEffectPacket(3004));
                    player.SendChatMessage("UPGRADESP_FAILED_SAVED", SayColorType.Purple);
                    player.SendTopscreenMessage("UPGRADESP_FAILED_SAVED", MsgPacketType.Whisper);
                }
                else
                {
                    wearable.Rarity = -2;
                    player.SendChatMessage("UPGRADESP_DESTROYED", SayColorType.Purple);
                    player.SendTopscreenMessage("UPGRADESP_DESTROYED", MsgPacketType.Whisper);
                    player.SendPacket(wearable.GenerateIvnPacket());
                }
            }
            else if (rnd < Configuration.UpgradeSp.UpFail[e.Item.Upgrade])
            {
                if (e.Protection == UpgradeProtection.Protected)
                {
                    player.SendPacket(player.GenerateEffectPacket(3004));
                }

                player.SendChatMessage("UPGRADESP_FAILED", SayColorType.Purple);
                player.SendTopscreenMessage("UPGRADESP_FAILED", MsgPacketType.Whisper);
            }
            else
            {
                if (e.Protection == UpgradeProtection.Protected)
                {
                    player.SendPacket(player.GenerateEffectPacket(3004));
                }

                player.SendPacket(player.GenerateEffectPacket(3005));
                player.SendChatMessage("UPGRADESP_SUCCESS", SayColorType.Purple);
                player.SendTopscreenMessage("UPGRADESP_SUCCESS", MsgPacketType.Whisper);
                /* if (Upgrade < 5)
                 {
                     CharacterSession.Character.Inventory.RemoveItemAmount(
                         Item.Morph <= 16 ? greenSoulVnum : dragonSkinVnum, soul[Upgrade]);
                 }
                 else if (Upgrade < 10)
                 {
                     CharacterSession.Character.Inventory.RemoveItemAmount(
                         Item.Morph <= 16 ? redSoulVnum : dragonBloodVnum, soul[Upgrade]);
                 }
                 else if (Upgrade < 15)
                 {
                     CharacterSession.Character.Inventory.RemoveItemAmount(
                         Item.Morph <= 16 ? blueSoulVnum : dragonHeartVnum, soul[Upgrade]);
                 }*/

                wearable.Upgrade++;
                if (wearable.Upgrade > 8)
                {
                    // CharacterSession.Character.Family?.InsertFamilyLog(FamilyLogType.ItemUpgraded,
                    //     CharacterSession.Character.Name, itemVNum: wearable.ItemVNum, upgrade: wearable.Upgrade);
                }

                player.SendPacket(wearable.GenerateIvnPacket());
            }

            player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow);
        }
    }
}