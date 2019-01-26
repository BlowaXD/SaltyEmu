using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage.Handling;
using ChickenAPI.Game.Mates.Events;
using ChickenAPI.Packets.Game.Client.Player;

namespace SaltyEmu.BasicPlugin.ItemUsageHandlers.Handler.BoxItem
{
    public class PearlPetsReleaseHandler
    {
        private static readonly Logger Log = Logger.GetLogger<PearlPetsReleaseHandler>();

        [UseItemEffect(1, ItemType.Box)]
        public static void ReleasePets(IPlayerEntity player, InventoryUseItemEvent e)
        {
            if (e.Option == 0)
            {
                player.GenerateQna((new ClientGuriPacket { Type = 300, Argument = 8023, VisualId = e.Item.Slot }), "ASK_RELEASE_PET");
                return;
            }

            player.EmitEvent(new AddPetEvent { MonsterVnum = e.Item.Item.EffectValue, MateType = e.Item.Item.ItemSubType == 1 ? MateType.Partner : MateType.Pet });
        }
    }
}