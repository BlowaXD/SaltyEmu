using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Features.Movement.Extensions
{
    public static class MovablePlayerExtensions
    {
        public static CondPacketBase GenerateCondPacket(this IPlayerEntity entity)
        {
            return new CondPacketBase
            {
                CanAttack = entity.Battle.CanAttack,
                CanMove = entity.Battle.CanMove,
                VisualType = VisualType.Character,
                VisualId = entity.Character.Id,
                Speed = entity.Movable.Speed
            };
        }
    }
}
