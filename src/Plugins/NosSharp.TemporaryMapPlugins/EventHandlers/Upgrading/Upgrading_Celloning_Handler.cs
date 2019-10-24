using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Shops.Extensions;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Upgrading_Celloning_Handler : GenericEventPostProcessorBase<CellonItemEvent>
    {
        public Upgrading_Celloning_Handler(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(CellonItemEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }


            await player.GoldLess(e.GoldAmount);
            //Session.Character.Inventory.RemoveItemAmount(cellon.ItemVNum);

            // GENERATE OPTION
            EquipmentOptionDto option = CellonGeneratorHelper.Instance.GenerateOption(e.Cellon.Item.EffectValue);

            // FAIL
            /*
            if (option == null || e.Jewelry.EquipmentOptions.Any(s => s.Type == option.Type))
            {
                player.SendTopscreenMessage("CELLONING_FAILED", MessageType.White);
                player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow);
                return;
            }

            // SUCCESS
            e.Jewelry.EquipmentOptions.Add(option);
            */
            await player.SendTopscreenMessage("CELLONING_SUCCESS", MessageType.White);
            await player.SendPacketAsync(player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow));
        }
    }
}