using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Game.Client.Player;

namespace ChickenAPI.Game.Effects
{
    public static class EntityExtensionEffect
    {
        public static EffectPacket GenerateEffectPacket(this IEntity entity, long id)
        {
            long visualId = 0;
            var visualType = VisualType.Character;
            switch (entity)

            {
                case IPlayerEntity player:
                    visualId = player.Character.Id;
                    visualType = VisualType.Character;
                    break;
                case IMonsterEntity monster:
                    visualId = monster.MapMonster.Id;
                    visualType = VisualType.Monster;
                    break;
                case INpcEntity npc:
                    visualId = npc.MapNpc.Id;
                    visualType = VisualType.Monster;
                    break;
            }

            return new EffectPacket
            {
                CharacterId = visualId,
                EffectId = id,
                EffectType = visualType
            };
        }
    }
}