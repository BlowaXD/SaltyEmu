using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Game.Server.Entities;

namespace ChickenAPI.Game.Inventory.Extensions
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
                        Speed = monster.Speed,
                        MapX = monster.Position.X,
                        MapY = monster.Position.Y
                    };

                case INpcEntity npc:
                    return new MvPacket
                    {
                        VisualType = VisualType.Npc,
                        VisualId = npc.MapNpc.Id,
                        Speed = npc.Speed,
                        MapX = npc.Position.X,
                        MapY = npc.Position.Y
                    };
                case IPlayerEntity player:
                    return new MvPacket
                    {
                        VisualType = VisualType.Character,
                        VisualId = player.Character.Id,
                        Speed = player.Speed,
                        MapX = player.Position.X,
                        MapY = player.Position.Y
                    };
                default:
                    return null;
            }
        }
    }
}