using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Packets.Game.Server.Battle;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class SuPacketExtensions
    {
        public static SuPacket GenerateSuPacket(this IBattleEntity entity, HitRequest hit)
        {
            BattleComponent battleTarget = hit.Target.Battle;
            return new SuPacket
            {
                VisualType = entity.Type,
                VisualId = entity.Id,
                HitMode = hit.HitMode,
                Damage = hit.Damages,
                HpPercentage = (byte)((battleTarget.Hp - hit.Damages < 0 ? 0 : battleTarget.Hp - hit.Damages) / (float)battleTarget.HpMax * 100), // factorise this code with extension
                PositionX = entity.Movable.Actual.X,
                PositionY = entity.Movable.Actual.X,
                TargetIsAlive = battleTarget.Hp > 0,
                AttackAnimation = hit.UsedSkill.AttackAnimation,
                SkillCooldown = hit.UsedSkill.Cooldown,
                SkillEffect = hit.UsedSkill.Effect,
                SkillVnum = hit.UsedSkill.Id,
                SkillTypeMinusOne = hit.UsedSkill.SkillType - 1,
                TargetVisualType = hit.Target.Type,
                TargetId = hit.Target.Id
            };
        }
    }
}