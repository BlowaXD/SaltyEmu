using ChickenAPI.Game.Movements;
using ChickenAPI.Packets.ServerPackets.Entities;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class MovableEntityExtensions
    {
        public static MovePacket GenerateMvPacket(this IMovableEntity entity)
        {
            return new MovePacket
            {
                VisualType = entity.Type,
                VisualEntityId = entity.Id,
                Speed = entity.Speed,
                MapX = entity.Position.X,
                MapY = entity.Position.Y
            };
        }
    }
}