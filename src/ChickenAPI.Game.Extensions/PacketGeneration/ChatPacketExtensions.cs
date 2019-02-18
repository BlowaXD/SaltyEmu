using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Game.Server.Chat;

namespace ChickenAPI.Game.Extensions.PacketGeneration
{
    public static class ChatPacketExtensions
    {
        public static SayItemPacket GenerateSayItemPacket(this IPlayerEntity player, string prefix, string message, ItemInstanceDto item)
        {
            return new SayItemPacket
            {
                CharacterName = player.Character.Name,
                GlobalPrefix = prefix, // todo i18n
                ItemName = item.Item.Name, // todo i18n
                OratorSlot = 0, // looks like bullshit and useless
                VisualId = player.Id,
                VisualType = player.Type,
                Message = message.Replace(' ', '^'),
                ItemData = new SayItemPacket.SayItemSubPacket
                {
                    IconId = item.Item.Type == InventoryType.Equipment ? (long?)null : item.ItemId,
                    EquipmentInfo = item.Item.Type == InventoryType.Equipment ? item.GenerateEInfoPacket() : null
                },
            };
        }
    }
}