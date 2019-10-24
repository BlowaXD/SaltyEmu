using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Entities;

namespace ChickenAPI.Game.Movements.Extensions
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
                        IsSitting = monster.IsSitting
                    };

                case INpcEntity npc:
                    return new RestPacket
                    {
                        VisualType = VisualType.Npc,
                        VisualId = npc.MapNpc.Id,
                        IsSitting = npc.IsSitting
                    };
                case IPlayerEntity player:
                    return new RestPacket
                    {
                        VisualType = VisualType.Player,
                        VisualId = player.Id,
                        IsSitting = player.IsSitting
                    };
                default:
                    return null;
            }
        }
    }
}