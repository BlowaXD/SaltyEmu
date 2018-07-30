using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("cond")]
    public class CondPacketBase : PacketBase
    {
        public CondPacketBase(IPlayerEntity entity)
        {
            var character = entity.GetComponent<CharacterComponent>();

            VisualType = VisualType.Character;
            VisualId = character.Id;
            CanAttack = entity.Battle.CanAttack;
            CanMove = entity.Battle.CanMove;
            Speed = entity.Movable.Speed;
        }
        #region Properties

        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public bool CanAttack { get; set; }

        [PacketIndex(3)]
        public bool CanMove { get; set; }

        [PacketIndex(4)]
        public byte Speed { get; set; }

        #endregion
    }
}
