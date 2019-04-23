using ChickenAPI.Game._ECS.Entities;
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