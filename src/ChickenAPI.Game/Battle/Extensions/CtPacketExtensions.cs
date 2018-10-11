using ChickenAPI.Data.Skills;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Packets.Game.Server.Battle;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class CtPacketExtensions
    {
        public static CtPacket GenerateCtPacket(IEntity entity, IEntity target, SkillDto skill)
        {
            return new CtPacket
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
}