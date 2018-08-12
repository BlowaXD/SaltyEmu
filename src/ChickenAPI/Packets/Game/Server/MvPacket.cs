using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Packets.Game.Server
{
    [PacketHeader("mv")]
    public class MvPacket : PacketBase
    {
        public MvPacket(IEntity entity)
        {
            switch (entity)
            {
                case IMonsterEntity monster:
                    VisualType = VisualType.Monster;
                    VisualId = monster.MapMonster.Id;
                    Speed = monster.Movable.Speed;
                    MapX = monster.Movable.Actual.X;
                    MapY = monster.Movable.Actual.Y;
                    return;
                case INpcEntity npc:
                    VisualType = VisualType.Npc;
                    VisualId = npc.MapNpc.Id;
                    Speed = npc.Movable.Speed;
                    MapX = npc.Movable.Actual.X;
                    MapY = npc.Movable.Actual.Y;
                    return;
                case IPlayerEntity player:
                    VisualType = VisualType.Character;
                    VisualId = player.Character.Id;
                    Speed = player.Movable.Speed;
                    MapX = player.Movable.Actual.X;
                    MapY = player.Movable.Actual.Y;
                    return;
                default:
                    return;
            }
        }

        public MvPacket(IPlayerEntity entity)
        {
            VisualType = VisualType.Character;
            VisualId = entity.Character.Id;
            Speed = entity.Movable.Speed;
            MapX = entity.Movable.Actual.X;
            MapY = entity.Movable.Actual.Y;
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