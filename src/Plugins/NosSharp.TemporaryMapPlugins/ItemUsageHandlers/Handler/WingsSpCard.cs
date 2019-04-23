using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage;
using ChickenAPI.Packets.Old.Game.Client.Inventory;

namespace SaltyEmu.BasicPlugin.ItemUsageHandlers.Handler
{
    public class WingsSpCardHandler : IUseItemRequestHandlerAsync
    {
        private static readonly Logger Log = Logger.GetLogger<WingsSpCardHandler>();

        public ItemType Type => ItemType.Special;
        public long EffectId => 650;

        public async Task Handle(IPlayerEntity player, InventoryUseItemEvent e)
        {
            if (!player.IsTransformedSp)
            {
                return;
            }

            if (e.Option != 0)
            {
                return;
            }

            await player.SendQuestionAsync(new UiPacket
            {
                VisualType = VisualType.Player,
                CharacterId = player.Id,
                InventoryType = e.Item.Type,
                InventorySlot = (byte)e.Item.Slot,
                Unknown2 = 3,
                Unknown3 = 0
            }, "ASK_WINGS_CHANGE");
            return;

            // EmitEvent(new ChangeSpWings)
        }
    }
}