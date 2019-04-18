using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Game.Server.Battle;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class CtPacketExtensions
    {
        public static CtPacket GenerateCtPacket(this IBattleEntity entity, IEntity target, SkillDto skill) =>
            new CtPacket
            {
                CastAnimationId = skill.CastAnimation,
                CastEffect = skill.CastEffect,
                SkillId = skill.Id,
                VisualType = entity.Type,
                VisualId = entity.Id,
                TargetVisualType = target.Type,
                TargetId = target.Id
            };
    }
}