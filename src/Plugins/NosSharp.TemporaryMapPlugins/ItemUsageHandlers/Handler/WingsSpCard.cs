using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage;
using ChickenAPI.Packets.ClientPackets.Inventory;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.ItemUsageHandlers.Handler
{
    public class WingsSpCardHandler : IUseItemRequestHandlerAsync
    {
        // private static readonly Logger Log = Logger.GetLogger<WingsSpCardHandler>();

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

            await player.SendQuestionAsync(new UseItemPacket
            {
                VisualType = VisualType.Player,
                VisualId = player.Id,
                Type = e.Item.Type,
                Slot = (byte)e.Item.Slot,
                Parameter = 3,
                Mode = 0,
            }, "ASK_WINGS_CHANGE");
            return;

            // EmitEvent(new ChangeSpWings)
        }
    }
}