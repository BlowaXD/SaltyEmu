using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Entities;

namespace ChickenAPI.Game.Packets.Extensions
{
    public static class MovableEntityExtensions
    {
        public static MvPacket GenerateMvPacket(this IEntity entity)
        {
            switch (entity)
            {
                case IMonsterEntity monster:
                    return new MvPacket
                    {
                        VisualType = VisualType.Monster,
                        VisualId = monster.MapMonster.Id,
                        Speed = monster.Movable.Speed,
                        MapX = monster.Movable.Actual.X,
                        MapY = monster.Movable.Actual.Y
                    };

                case INpcEntity npc:
                    return new MvPacket
                    {
                        VisualType = VisualType.Npc,
                        VisualId = npc.MapNpc.Id,
                        Speed = npc.Movable.Speed,
                        MapX = npc.Movable.Actual.X,
                        MapY = npc.Movable.Actual.Y
                    };
                case IPlayerEntity player:
                    return new MvPacket
                    {
                        VisualType = VisualType.Character,
                        VisualId = player.Character.Id,
                        Speed = player.Movable.Speed,
                        MapX = player.Movable.Actual.X,
                        MapY = player.Movable.Actual.Y
                    };
                default:
                    return null;
            }
        }
    }
}