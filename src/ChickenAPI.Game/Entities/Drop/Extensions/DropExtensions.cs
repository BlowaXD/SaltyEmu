using ChickenAPI.Packets.Game.Server.Entities;

namespace ChickenAPI.Game.Entities.Drop.Extensions
{
    public static class DropExtensions
    {
        public static DropPacket GenerateDropPacket(this IDropEntity drop) =>
            new DropPacket
            {
                ItemVnum = drop.ItemVnum,
                Quantity = drop.Quantity,
                PositionX = drop.Position.X,
                PositionY = drop.Position.Y,
                TransportId = drop.Id,
                IsQuestDrop = drop.IsQuestDrop,
                Unknown = 0,
                Unknown2 = -1
            };
    }
}