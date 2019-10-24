using ChickenAPI.Packets.ServerPackets.Entities;

namespace ChickenAPI.Game.Entities.Drop.Extensions
{
    public static class DropExtensions
    {
        public static DropPacket GenerateDropPacket(this IDropEntity drop) =>
            new DropPacket
            {
                VisualId = drop.Id,
                Amount = (short)drop.Quantity,
                VNum = (short)drop.ItemVnum,
                PositionX = drop.Position.X,
                PositionY = drop.Position.Y,
                Unknown = (byte)(drop.IsQuestDrop ? 1 : 0),
                OwnerId = drop.Id,
            };
    }
}