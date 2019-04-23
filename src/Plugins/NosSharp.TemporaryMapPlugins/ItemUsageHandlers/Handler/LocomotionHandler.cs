using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage;
using ChickenAPI.Game.Locomotion.Events;

namespace SaltyEmu.BasicPlugin.ItemUsageHandlers.Handler
{
    public class LocomotionHandler : IUseItemRequestHandlerAsync
    {
        private readonly ILogger _log;

        public LocomotionHandler(ILogger log)
        {
            _log = log;
        }

        public ItemType Type => ItemType.Special;

        public long EffectId => 1000;

        public async Task Handle(IPlayerEntity player, InventoryUseItemEvent e)
        {
            if (!player.IsTransformedLocomotion)
            {
                if (e.Option == 0)
                {
                    // await player.SendDelayAsync(3000, DelayPacketType.Locomotion, $"#u_i^1^{player.Character.Id}^{(byte)e.Item.Type}^{e.Item.Slot}^2");
                    return;
                }

                await player.EmitEventAsync(new LocomotionTransformEvent { Item = e.Item });
                return;
            }

            await player.EmitEventAsync(new LocomotionUntransformEvent { });
        }
    }
}