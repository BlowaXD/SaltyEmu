using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Maths;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Shops.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Upgrading
{
    public class Upgrading_UpgradeEquipment_Handler : GenericEventPostProcessorBase<UpgradeEquipmentEvent>
    {
        private readonly IGameConfiguration _configuration;
        private readonly IRandomGenerator _randomGenerator;

        public Upgrading_UpgradeEquipment_Handler(IGameConfiguration configuration, IRandomGenerator randomGenerator)
        {
            _configuration = configuration;
            _randomGenerator = randomGenerator;
        }

        protected override async Task Handle(UpgradeEquipmentEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }
            short[] upfail = e.Item.Rarity >= 8 ? _configuration.UpgradeItem.UpFailR8 : _configuration.UpgradeItem.UpFail;
            short[] upfix = e.Item.Rarity >= 8 ? _configuration.UpgradeItem.UpFixR8 : _configuration.UpgradeItem.UpFix;
            int[] goldprice = e.Item.Rarity >= 8 ? _configuration.UpgradeItem.GoldPriceR8 : _configuration.UpgradeItem.GoldPrice;
            short[] cella = e.Item.Rarity >= 8 ? _configuration.UpgradeItem.CellaAmountR8 : _configuration.UpgradeItem.CellaAmount;
            short[] gem = e.Item.Rarity >= 8 ? _configuration.UpgradeItem.GemAmountR8 : _configuration.UpgradeItem.GemAmount;

            if (e.HasAmulet == FixedUpMode.HasAmulet && e.Item.IsFixed)
            {
                upfix = new short[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }

            if (e.Item.IsFixed && e.HasAmulet == FixedUpMode.None)
            {
                await player.SendChatMessageAsync("ITEM_IS_FIXED", SayColorType.Yellow);
                await player.SendPacketAsync(player.GenerateShopEndPacket(ShopEndPacketType.CloseWindow));
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
                    if (player.Character.Gold < (long)(goldprice[e.Item.Upgrade] * _configuration.UpgradeItem.ReducedPriceFactor))
                    {
                        await player.SendChatMessageAsync("NOT_ENOUGH_MONEY", SayColorType.Yellow);
                        return;
                    }

                    if (player.Inventory.GetItemQuantityById(_configuration.UpgradeItem.CellaVnum) < cella[e.Item.Upgrade] * _configuration.UpgradeItem.ReducedPriceFactor)
                    {
                        await player.SendChatMessageAsync("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                        return;
                    }

                    if (e.Protection == UpgradeProtection.Protected && !e.IsCommand &&
                        player.Inventory.GetItemQuantityById(_configuration.UpgradeItem.GoldScrollVnum) < 1)
                    {
                        await player.SendChatMessageAsync("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                        return;
                    }

                    if (player.Inventory.GetItemQuantityById(e.Item.Upgrade < 5 ? _configuration.UpgradeItem.GemVnum : _configuration.UpgradeItem.GemFullVnum) < gem[e.Item.Upgrade])
                    {
                        await player.SendChatMessageAsync("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                        return;
                    }

                    // player.inventory.removeitemamount( upgrade < 5 ? gemvnum : gemfullvnum , gem[e.item.upgrade]);

                    if (e.Protection == UpgradeProtection.Protected && !e.IsCommand)
                    {
                        // remove Item gold scroll x 1
                        await player.SendPacketAsync(
                            player.GenerateShopEndPacket(player.Inventory.HasItem(_configuration.UpgradeItem.GoldScrollVnum)
                                ? ShopEndPacketType.CloseSubWindow
                                : ShopEndPacketType.CloseWindow));
                    }

                    if (e.HasAmulet == FixedUpMode.HasAmulet && e.Item.IsFixed)
                    {
                        ItemInstanceDto amulet = player.Inventory.GetWeared(EquipmentType.Amulet);
                        /*amulet.DurabilityPoint -= 1;
                        if (amulet.DurabilityPoint <= 0)
                        {
                            session.Character.DeleteItemByItemInstanceId(amulet.Id);
                            await session.SendPacketAsync($"info {Language.Instance.GetMessageFromKey("AMULET_DESTROYED")}");
                            await session.SendPacketAsync(session.Character.GenerateEquipment());
                        }*/
                    }

                    await player.GoldLess(goldprice[e.Item.Upgrade] * (long)_configuration.UpgradeItem.ReducedPriceFactor);
                    //session.Character.Inventory.RemoveItemAmount(cellaVnum,
                    //    (int)(cella[Upgrade] * reducedpricefactor));
                    break;

                case UpgradeMode.Normal:

                    // TODO: Normal Item Amount
                    Log.Info("Vnum:");
                    Log.Info(_configuration.UpgradeItem.CellaVnum.ToString());
                    Log.Info("Amount:");
                    Log.Info(cella[e.Item.Upgrade].ToString());
                    // Not work Idk why
                    /* if (player.Inventory.GetItemQuantityById(Configuration.UpgradeItem.CellaVnum) < cella[e.Item.Upgrade])
                     {
                         player.SendChatMessageAsync("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                         return;
                     }*/

                    if (player.Character.Gold < goldprice[e.Item.Upgrade])
                    {
                        await player.SendChatMessageAsync("NOT_ENOUGH_MONEY", SayColorType.Yellow);
                        return;
                    }

                    if (e.Protection == UpgradeProtection.Protected && !e.IsCommand &&
                        player.Inventory.GetItemQuantityById(_configuration.UpgradeItem.NormalScrollVnum) < 1)
                    {
                        await player.SendChatMessageAsync("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                        return;
                    }

                    if (player.Inventory.GetItemQuantityById(e.Item.Upgrade < 5 ? _configuration.UpgradeItem.GemVnum : _configuration.UpgradeItem.GemFullVnum) < gem[e.Item.Upgrade])
                    {
                        await player.SendChatMessageAsync("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                        return;
                    }

                    // player.inventory.removeitemamount( upgrade < 5 ? gemvnum : gemfullvnum , gem[e.item.upgrade]);

                    if (e.Protection == UpgradeProtection.Protected && !e.IsCommand)
                    {
                        // remove Item normal scroll x 1
                        await player.SendPacketAsync(player.GenerateShopEndPacket(player.Inventory.HasItem(_configuration.UpgradeItem.NormalScrollVnum)
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
                            await session.SendPacketAsync($"info {Language.Instance.GetMessageFromKey("AMULET_DESTROYED")}");
                            await session.SendPacketAsync(session.Character.GenerateEquipment());
                        }*/
                    }

                    //session.Character.Inventory.RemoveItemAmount(cellaVnum, cella[Upgrade]);
                    await player.GoldLess(goldprice[e.Item.Upgrade]);
                    break;
            }

            ItemInstanceDto wearable = e.Item;

            double rnd = _randomGenerator.Next();

            if (e.Item.Rarity == 8)
            {
                if (rnd < upfail[e.Item.Upgrade])
                {
                    if (e.Protection == UpgradeProtection.None)
                    {
                        await player.SendChatMessageAsync("UPGRADE_FAILED", SayColorType.Purple);
                        await player.SendTopscreenMessage("UPGRADE_FAILED", MessageType.Whisper);
                        //session.Character.DeleteItemByItemInstanceId(Id);
                    }
                    else
                    {
                        await player.BroadcastAsync(player.GenerateEffectPacket(3004));
                        await player.SendChatMessageAsync("SCROLL_PROTECT_USED", SayColorType.Purple);
                        await player.SendTopscreenMessage("UPGRADE_FAILED_ITEM_SAVED", MessageType.Whisper);
                    }
                }
                else if (rnd < upfix[e.Item.Upgrade])
                {
                    await player.BroadcastAsync(player.GenerateEffectPacket(3004));
                    wearable.IsFixed = true;
                    await player.SendChatMessageAsync("UPGRADE_FIXED", SayColorType.Purple);
                    await player.SendTopscreenMessage("UPGRADE_FIXED", MessageType.Whisper);
                }
                else
                {
                    await player.BroadcastAsync(player.GenerateEffectPacket(3005));
                    await player.SendChatMessageAsync("UPGRADE_SUCCESS", SayColorType.Green);
                    await player.SendTopscreenMessage("UPGRADE_SUCCESS", MessageType.Whisper);
                    wearable.Upgrade++;
                    if (wearable.Upgrade > 4)
                    {
                        //session.Character.Family?.InsertFamilyLog(FamilyLogType.ItemUpgraded, session.Character.Name,
                        //   itemVNum: wearable.ItemVNum, upgrade: wearable.Upgrade);
                    }

                    await player.ActualizeUiInventorySlot(wearable.Type, wearable.Slot);
                }
            }
            else
            {
                if (rnd < upfix[e.Item.Upgrade])
                {
                    await player.BroadcastAsync(player.GenerateEffectPacket(3004));
                    wearable.IsFixed = true;
                    await player.SendChatMessageAsync("UPGRADE_FIXED", SayColorType.Purple);
                    await player.SendTopscreenMessage("UPGRADE_FIXED", MessageType.Whisper);
                }
                else if (rnd < upfail[e.Item.Upgrade] + upfix[e.Item.Upgrade])
                {
                    if (e.Protection == UpgradeProtection.None)
                    {
                        await player.SendChatMessageAsync("UPGRADE_FAILED", SayColorType.Purple);
                        await player.SendTopscreenMessage("UPGRADE_FAILED", MessageType.Whisper);
                        //session.Character.DeleteItemByItemInstanceId(Id);
                    }
                    else
                    {
                        await player.BroadcastAsync(player.GenerateEffectPacket(3004));
                        await player.SendChatMessageAsync("SCROLL_PROTECT_USED", SayColorType.Purple);
                        await player.SendTopscreenMessage("UPGRADE_FAILED_ITEM_SAVED", MessageType.Whisper);
                    }
                }
                else
                {
                    await player.BroadcastAsync(player.GenerateEffectPacket(3005));
                    await player.SendChatMessageAsync("UPGRADE_SUCCESS", SayColorType.Green);
                    await player.SendTopscreenMessage("UPGRADE_SUCCESS", MessageType.Whisper);
                    wearable.Upgrade++;
                    if (wearable.Upgrade > 4)
                    {
                        //session.Character.Family?.InsertFamilyLog(FamilyLogType.ItemUpgraded, session.Character.Name,
                        //   itemVNum: wearable.ItemVNum, upgrade: wearable.Upgrade);
                    }

                    await player.ActualizeUiInventorySlot(wearable.Type, wearable.Slot);
                }
            }

            await player.SendPacketAsync(player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow));
        }
    }
}