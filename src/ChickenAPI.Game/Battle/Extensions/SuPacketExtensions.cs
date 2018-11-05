using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Packets.Game.Server.Battle;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class SuPacketExtensions
    {
        public static SuPacket GenerateSuPacket(this IBattleEntity entity, HitRequest hit, ushort damages)
        {
            return new SuPacket
            {
                VisualType = entity.Type,
                VisualId = entity.Id,
                HitMode = hit.HitMode,
                Damage = damages,
                HpPercentage = entity.HpPercentage, // factorise this code with extension
                PositionX = entity.Movable.Actual.X,
                PositionY = entity.Movable.Actual.X,
                TargetIsAlive = hit.Target.IsAlive,
                AttackAnimation = hit.UsedSkill.AttackAnimation,
                SkillCooldown = hit.UsedSkill.Cooldown,
                SkillEffect = hit.UsedSkill.Effect,
                SkillVnum = hit.UsedSkill.Id,
                SkillTypeMinusOne = hit.UsedSkill.SkillType - 1,
                TargetVisualType = hit.Target.Type,
                TargetId = hit.Target.Id
            };
        }

        /// <summary>
        /// Use this extension only if your damages are lower than <value>65535</value> (ushort limit)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="hit"></param>
        /// <returns></returns>
        public static SuPacket GenerateSuPacket(this IBattleEntity entity, HitRequest hit)
        {
            return GenerateSuPacket(entity, hit, (ushort)hit.Damages);
        }
    }
}