using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Game.Server.Entities;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Movement.Extensions
{
    public static class RestEntityExtensions
    {
        public static RestPacket GenerateRestPacket(this IEntity entity)
        {
            switch (entity)
            {
                case IMonsterEntity monster:
                    return new RestPacket
                    {
                        VisualType = VisualType.Monster,
                        VisualId = monster.MapMonster.Id,
                        IsSitting = monster.Movable.IsSitting
                    };

                case INpcEntity npc:
                    return new RestPacket
                    {
                        VisualType = VisualType.Npc,
                        VisualId = npc.MapNpc.Id,
                        IsSitting = npc.Movable.IsSitting
                    };
                case IPlayerEntity player:
                    return new RestPacket
                    {
                        VisualType = VisualType.Character,
                        VisualId = player.Character.Id,
                        IsSitting = player.Movable.IsSitting
                    };
                default:
                    return null;
            }
        }
    }
}