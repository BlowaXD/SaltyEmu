using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Movements.Extensions
{
    public static class MovablePlayerExtensions
    {
        public static CondPacketBase GenerateCondPacket(this IPlayerEntity entity) => new CondPacketBase
        {
            NoAttack = !entity.CanAttack,
            NoMove = !entity.CanMove,
            VisualType = entity.Type,
            VisualId = entity.Id,
            Speed = entity.Speed
        };
    }
}