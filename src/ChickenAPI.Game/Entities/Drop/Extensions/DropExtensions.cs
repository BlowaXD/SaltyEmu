using ChickenAPI.Packets.Game.Server.Entities;

namespace ChickenAPI.Game.Entities.Drop.Extensions
{
    public static class DropExtensions
    {
        private static long _transportId = 100000;

        public static long GenerateTransportId() => _transportId++;

        public static DropPacket GenerateDropPacket(this IDropEntity drop)
        {
            return new DropPacket
            {
                ItemVnum = drop.ItemVnum,
                Quantity = drop.Quantity,
                PositionX = drop.Position.X,
                PositionY = drop.Position.Y,
                TransportId = drop.Id,
                IsQuestDrop = drop.IsQuestDrop,
                Unknown = 0,
                Unknown2 = -1,
            };
        }
    }
}