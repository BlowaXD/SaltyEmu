using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage;
using ChickenAPI.Game.Mates.Events;
using ChickenAPI.Packets.ClientPackets.Player;

namespace SaltyEmu.BasicPlugin.ItemUsageHandlers.Handler.BoxItem
{
    public class PearlPetsReleaseHandler : IUseItemRequestHandlerAsync
    {
        public ItemType Type => ItemType.Box;
        public long EffectId => 1;

        public async Task Handle(IPlayerEntity player, InventoryUseItemEvent e)
        {
            if (e.Option == 0)
            {
                // todo clean
                await player.SendQuestionAsync(new ClientGuriPacket { Type = 300, Argument = 8023, VisualId = e.Item.Slot }, "ASK_RELEASE_PET");
                return;
            }

            await player.EmitEventAsync(new AddPetEvent { MonsterVnum = e.Item.Item.EffectValue, MateType = e.Item.Item.ItemSubType == 1 ? MateType.Partner : MateType.Pet });
        }
    }
}