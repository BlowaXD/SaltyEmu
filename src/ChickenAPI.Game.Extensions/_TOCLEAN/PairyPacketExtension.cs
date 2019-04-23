using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Old.Game.Server.Inventory;

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
                    CharacterId = player.Character.Id,
                    FairyMoveType = 0,
                    ElementType = 0,
                    FairyLevel = 0
                };
            }

            // todo buffs

            return new PairyPacket
            {
                VisualType = VisualType.Player,
                CharacterId = player.Character.Id,
                ElementType = fairy.ElementType,
                FairyLevel = fairy.Upgrade,
                FairyMoveType = 4 // todo enum 
            };
        }
    }
}