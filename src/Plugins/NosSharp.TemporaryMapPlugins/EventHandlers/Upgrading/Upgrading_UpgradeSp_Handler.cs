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

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Upgrading_UpgradeSp_Handler : GenericEventPostProcessorBase<UpgradeSpecialistEvent>
    {
        private readonly IGameConfiguration _configuration;
        private readonly IRandomGenerator _random;

        public Upgrading_UpgradeSp_Handler(IGameConfiguration configuration, IRandomGenerator random)
        {
            _configuration = configuration;
            _random = random;
        }

        protected override async Task Handle(UpgradeSpecialistEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            if (player.Inventory.GetItemQuantityById(_configuration.UpgradeSp.FullmoonVnum) < _configuration.UpgradeSp.FullMoon[e.Item.Upgrade])
            {
                await player.SendChatMessageAsync("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                return;
            }

            if (player.Inventory.GetItemQuantityById(_configuration.UpgradeSp.FeatherVnum) < _configuration.UpgradeSp.Feather[e.Item.Upgrade])
            {
                await player.SendChatMessageAsync("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                return;
            }

            if (player.Character.Gold < _configuration.UpgradeSp.GoldPrice[e.Item.Upgrade])
            {
                await player.SendChatMessageAsync("NOT_ENOUGH_MONEY", SayColorType.Yellow);
                return;
            }

            if (e.Item.Upgrade < 5 ? e.Item.Level < 20 : e.Item.Upgrade < 10 ? e.Item.Level < 40 : e.Item.Level < 50)
            {
                await player.SendChatMessageAsync("LVL_REQUIRED", SayColorType.Purple);
                return;
            }

            if (player.Inventory.GetItemQuantityById(e.Item.Upgrade < 5 ? (e.Item.Item.Morph <= 16 ? _configuration.UpgradeSp.GreenSoulVnum : _configuration.UpgradeSp.DragonSkinVnum) :
                    e.Item.Upgrade < 10 ? (e.Item.Item.Morph <= 16 ? _configuration.UpgradeSp.RedScrollVnum : _configuration.UpgradeSp.DragonBloodVnum) :
                    (e.Item.Item.Morph <= 16 ? _configuration.UpgradeSp.BlueSoulVnum : _configuration.UpgradeSp.DragonHeartVnum)) <
                _configuration.UpgradeSp.Soul[e.Item.Upgrade])
            {
                await player.SendChatMessageAsync("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                return;
            }

            if (e.Protection == UpgradeProtection.Protected)
            {
                if (player.Inventory.GetItemQuantityById(e.Item.Upgrade < 15
                    ? _configuration.UpgradeSp.BlueScrollVnum
                    : _configuration.UpgradeSp.RedScrollVnum) < 1)
                {
                    await player.SendChatMessageAsync("NOT_ENOUGH_ITEMS", SayColorType.Yellow);
                    return;
                }

                // remove Item Blue/red scroll x 1
                await player.SendPacketAsync(player.GenerateShopEndPacket(player.Inventory.HasItem(e.Item.Upgrade < 15
                    ? _configuration.UpgradeSp.BlueScrollVnum
                    : _configuration.UpgradeSp.RedScrollVnum)
                    ? ShopEndPacketType.CloseSubWindow
                    : ShopEndPacketType.CloseWindow));
            }

            await player.GoldLess(_configuration.UpgradeSp.GoldPrice[e.Item.Upgrade]);
            //CharacterSession.Character.Inventory.RemoveItemAmount(featherVnum, feather[Upgrade]);
            //CharacterSession.Character.Inventory.RemoveItemAmount(fullmoonVnum, fullmoon[Upgrade]);
            ItemInstanceDto wearable = e.Item;
            double rnd = _random.Next();

            if (rnd < _configuration.UpgradeSp.Destroy[e.Item.Upgrade])
            {
                if (e.Protection == UpgradeProtection.Protected)
                {
                    await player.SendPacketAsync(player.GenerateEffectPacket(3004));
                    await player.SendChatMessageAsync("UPGRADESP_FAILED_SAVED", SayColorType.Purple);
                    await player.SendTopscreenMessage("UPGRADESP_FAILED_SAVED", MsgPacketType.Whisper);
                }
                else
                {
                    wearable.Rarity = -2;
                    await player.SendChatMessageAsync("UPGRADESP_DESTROYED", SayColorType.Purple);
                    await player.SendTopscreenMessage("UPGRADESP_DESTROYED", MsgPacketType.Whisper);
                    await player.SendPacketAsync(wearable.GenerateIvnPacket());
                }
            }
            else if (rnd < _configuration.UpgradeSp.UpFail[e.Item.Upgrade])
            {
                if (e.Protection == UpgradeProtection.Protected)
                {
                    await player.SendPacketAsync(player.GenerateEffectPacket(3004));
                }

                await player.SendChatMessageAsync("UPGRADESP_FAILED", SayColorType.Purple);
                await player.SendTopscreenMessage("UPGRADESP_FAILED", MsgPacketType.Whisper);
            }
            else
            {
                if (e.Protection == UpgradeProtection.Protected)
                {
                    await player.SendPacketAsync(player.GenerateEffectPacket(3004));
                }

                await player.SendPacketAsync(player.GenerateEffectPacket(3005));
                await player.SendChatMessageAsync("UPGRADESP_SUCCESS", SayColorType.Purple);
                await player.SendTopscreenMessage("UPGRADESP_SUCCESS", MsgPacketType.Whisper);
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

                await player.SendPacketAsync(wearable.GenerateIvnPacket());
            }

            await player.SendPacketAsync(player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow));
        }
    }
}