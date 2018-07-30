using System;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("mv")]
    public class MvPacket : PacketBase
    {
        public MvPacket(IPlayerEntity entity)
        {
            VisualType = VisualType.Character;
            VisualId = entity.Character.Id;
            Speed = entity.Movable.Speed;
            MapX = entity.Movable.Actual.X;
            MapY = entity.Movable.Actual.Y;
        }

        public MvPacket(IEntity entity)
        {
            switch (entity.Type)
            {
                case EntityType.Monster:
                    VisualType = VisualType.Monster;
                    VisualId = entity.GetComponent<NpcMonsterComponent>().MapNpcMonsterId;
                    break;
                case EntityType.Player:
                    VisualType = VisualType.Character;
                    VisualId = entity.GetComponent<CharacterComponent>().Id;
                    break;
                case EntityType.Mate:
                    VisualType = VisualType.Npc;
                    break;
                case EntityType.Npc:
                    VisualType = VisualType.Npc;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var movable = entity.GetComponent<MovableComponent>();
            MapX = movable.Actual.X;
            MapY = movable.Actual.Y;
            Speed = movable.Speed;
        }

        public MvPacket()
        {
            throw new NotImplementedException();
        }

        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public short MapX { get; set; }

        [PacketIndex(3)]
        public short MapY { get; set; }

        [PacketIndex(4)]
        public short Speed { get; set; }
    }
}