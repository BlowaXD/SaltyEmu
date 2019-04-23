using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.ServerPackets.Player;

namespace ChickenAPI.Game.Movements.Extensions
{
    public static class MovablePlayerExtensions
    {
        public static CondPacket GenerateCondPacket(this IPlayerEntity entity) => new CondPacket
        {
            NoAttack = !entity.CanAttack,
            NoMove = !entity.CanMove,
            VisualType = entity.Type,
            VisualId = entity.Id,
            Speed = entity.Speed
        };
    }
}