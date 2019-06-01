using ChickenAPI.Data.Item;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Inventory;
using EquipmentType = ChickenAPI.Data.Enums.Game.Items.EquipmentType;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class PairyPacketExtension
    {
        public static PairyPacket GeneratePairyPacket(this IPlayerEntity player)
        {
            ItemInstanceDto fairy = player.Inventory.GetWeared(EquipmentType.Fairy);
            if (fairy == null)
            {
                return new PairyPacket
                {
                    VisualType = VisualType.Player,
                    VisualId = player.Character.Id,
                    FairyMoveType = 0,
                    Element = ElementType.Neutral,
                    ElementRate = 0,
                    Morph = -1,
                };
            }

            // todo buffs

            return new PairyPacket
            {
                VisualType = VisualType.Player,
                VisualId = player.Character.Id,
                Morph = (int)fairy.ItemId,
                Element = fairy.ElementType,
                ElementRate = fairy.Upgrade,
                FairyMoveType = 4 // todo enum 
            };
        }
    }
}