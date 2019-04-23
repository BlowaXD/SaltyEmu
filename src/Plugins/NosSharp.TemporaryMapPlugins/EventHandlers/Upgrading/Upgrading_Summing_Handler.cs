using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Maths;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Shops.Extensions;
using ChickenAPI.Packets.Old.Game.Client.Inventory;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Upgrading_Summing_Handler : GenericEventPostProcessorBase<SummingEvent>
    {
        private readonly IGameConfiguration _configuration;
        private readonly IRandomGenerator _random;

        public Upgrading_Summing_Handler(IRandomGenerator random, IGameConfiguration configuration)
        {
            _random = random;
            _configuration = configuration;
        }

        protected override async Task Handle(SummingEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            //session.Character.Inventory.RemoveItemAmount(sandVnum, (byte)sand[Upgrade + itemToSum.Upgrade]);
            await player.GoldLess(_configuration.Summing.GoldPrice[e.Item.Sum + e.SecondItem.Sum]);

            double rnd = _random.Next();
            if (rnd < _configuration.Summing.UpSucess[e.Item.Sum + e.SecondItem.Sum])
            {
                e.Item.Sum += (byte)(e.SecondItem.Sum + 1);
                e.Item.DarkResistance += (short)(e.SecondItem.DarkResistance + e.SecondItem.Item.DarkResistance);
                e.Item.LightResistance += (short)(e.SecondItem.LightResistance + e.SecondItem.Item.LightResistance);
                e.Item.WaterResistance += (short)(e.SecondItem.WaterResistance + e.SecondItem.Item.WaterResistance);
                e.Item.FireResistance += (short)(e.SecondItem.FireResistance + e.SecondItem.Item.FireResistance);
                //session.Character.DeleteItemByItemInstanceId(itemToSum.Id);
                await player.SendPacketAsync(new PdtiPacket { Unknow = 10, Unknow2 = 1, Unknow3 = 27, Unknow4 = 0, ItemVnum = e.Item.Item.Id, ItemUpgrade = e.Item.Sum });
                await player.SendChatMessageAsync("SUM_SUCCESS", SayColorType.Green);
                await player.SendTopscreenMessage("SUM_SUCCESS", MsgPacketType.Whisper);
                await player.SendGuri(GuriPacketType.AfterSumming, 1, 1324);
                await player.SendPacketAsync(e.Item?.GenerateIvnPacket());
            }
            else
            {
                await player.SendChatMessageAsync("SUM_FAILED", SayColorType.Purple);
                await player.SendTopscreenMessage("SUM_FAILED", MsgPacketType.Whisper);
                await player.SendGuri(GuriPacketType.AfterSumming, 1, 1332);
                //session.Character.DeleteItemByItemInstanceId(itemToSum.Id);
                //session.Character.DeleteItemByItemInstanceId(Id);
            }

            await player.SendPacketAsync(player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow));
            await player.BroadcastAsync(player.GenerateGuriPacket(GuriPacketType.Unknow2, 1));
        }
    }
}