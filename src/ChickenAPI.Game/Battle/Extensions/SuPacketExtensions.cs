using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Packets.Game.Server.Battle;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class SuPacketExtensions
    {
        public static SuPacket GenerateSuPacket(HitRequest hit)
        {
            var movable = hit.Sender.GetComponent<MovableComponent>();
            var battleTarget = hit.Target.GetComponent<BattleComponent>();
            return new SuPacket
            {
                HitMode = hit.HitMode,
                Damage = hit.Damages,
                HpPercentage = (byte)((battleTarget.Hp - hit.Damages < 0 ? 0 : battleTarget.Hp - hit.Damages) / (float)battleTarget.HpMax * 100),
                PositionX = movable.Actual.X,
                PositionY = movable.Actual.Y,
                TargetIsAlive = true,
                AttackAnimation = hit.UsedSkill.AttackAnimation,
                SkillCooldown = hit.UsedSkill.Cooldown,
                SkillEffect = hit.UsedSkill.Effect,
                SkillVnum = hit.UsedSkill.Id,
                SkillTypeMinusOne = hit.UsedSkill.SkillType - 1,
                VisualType = hit.Sender.Type,
                VisualId = hit.Sender.Id,
                TargetVisualType = hit.Target.Type,
                TargetId = hit.Target.Id
            };
        }
    }
}