using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Features.Movement.Extensions
{
    public static class MovablePlayerExtensions
    {
        public static CondPacketBase GenerateCondPacket(this IPlayerEntity entity) => new CondPacketBase
        {
            CanAttack = entity.Battle.CanAttack,
            CanMove = entity.Battle.CanMove,
            VisualType = VisualType.Character,
            VisualId = entity.Character.Id,
            Speed = entity.Movable.Speed
        };
    }
}