using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Player;

namespace ChickenAPI.Game.Effects
{
    public static class EntityExtensionEffect
    {
        public static EffectPacket GenerateEffectPacket(this IEntity entity, int id)
        {
            return new EffectPacket
            {
                VisualEntityId = entity.Id,
                EffectType = entity.Type,
                Id = id,
            };
        }
    }
}