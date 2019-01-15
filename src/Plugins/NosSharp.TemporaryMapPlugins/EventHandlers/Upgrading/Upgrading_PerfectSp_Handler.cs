using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Maths;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Shops.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Upgrading_PerfectSp_Handler : GenericEventPostProcessorBase<PerfectSPCardEvent>
    {
        private readonly IGameConfiguration _configuration;
        private readonly IRandomGenerator _random;

        public Upgrading_PerfectSp_Handler(IRandomGenerator random, IGameConfiguration configuration)
        {
            _random = random;
            _configuration = configuration;
        }

        protected override async Task Handle(PerfectSPCardEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            double rnd = _random.Next();
            if (rnd < _configuration.PerfectSp.UpSuccess[e.UpMode - 1])
            {
                byte type = (byte)_random.Next(0, 16), count = 1;
                switch (e.UpMode)
                {
                    case 4:
                        count = 2;
                        break;
                    case 5:
                        count = (byte)_random.Next(3, 6);
                        break;
                }

                player.SendPacket(player.GenerateEffectPacket(3005));

                int stoneup = (type < 3 ? e.SpCard.SpDamage :
                    type < 6 ? e.SpCard.SpDefence :
                    type < 9 ? e.SpCard.SpElement :
                    type < 12 ? e.SpCard.SpHP :
                    type == 12 ? e.SpCard.SpFire :
                    type == 13 ? e.SpCard.SpWater :
                    type == 14 ? e.SpCard.SpLight : e.SpCard.SpDark);

                stoneup += count;
                await player.SendTopscreenMessage("PERFECTSP_SUCCESS", MsgPacketType.White);
                await player.SendChatMessage("PERFECTSP_SUCCESS", SayColorType.Green);

                e.SpCard.SpStoneUpgrade++;
            }
            else
            {
                await player.SendTopscreenMessage("PERFECTSP_FAILURE", MsgPacketType.White);
                await player.SendChatMessage("PERFECTSP_FAILURE", SayColorType.Purple);
            }

            await player.ActualizeUiInventorySlot(e.SpCard.Type, e.SpCard.Slot);
            player.GoldLess(_configuration.PerfectSp.GoldPrice[e.UpMode - 1]);
            //CharacterSession.Character.Inventory.RemoveItemAmount(stonevnum, stoneprice[upmode - 1]);
            await player.SendPacketAsync(player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow));
        }
    }
}