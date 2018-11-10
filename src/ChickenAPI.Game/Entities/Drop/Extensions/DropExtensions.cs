using ChickenAPI.Packets.Game.Server.Entities;

namespace ChickenAPI.Game.Entities.Drop
{
    public static class DropExtensions
    {
        public static DropPacket GenerateDropPacket(this ItemDropEntity drop)
        {
            return new DropPacket
            {
                ItemVnum = drop.Item.Vnum,
                Quantity = drop.Quantity,
                PositionX = drop.Position.X,
                PositionY = drop.Position.Y,
                TransportId = drop.Id,
                IsQuestDrop = drop.IsQuest,
                Unknown = 0,
                Unknown2 = -1,
            };
        }
    }
}