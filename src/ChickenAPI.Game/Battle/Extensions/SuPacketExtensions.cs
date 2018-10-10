using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
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

            var su = new SuPacket
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
                SkillTypeMinusOne = hit.UsedSkill.SkillType - 1
            };
            switch (hit.Sender)
            {
                case IPlayerEntity player:
                    su.VisualType = VisualType.Character;
                    su.VisualId = player.Character.Id;
                    break;

                case INpcEntity npc:
                    su.VisualType = VisualType.Npc;
                    su.VisualId = npc.MapNpc.Id;
                    break;

                case IMonsterEntity monster:
                    su.VisualType = VisualType.Monster;
                    su.VisualId = monster.MapMonster.Id;
                    break;
            }
            switch (hit.Target)
            {
                case IPlayerEntity player:
                    su.TargetVisualType = VisualType.Character;
                    su.TargetId = player.Character.Id;
                    break;

                case INpcEntity npc:
                    su.TargetVisualType = VisualType.Npc;
                    su.TargetId = npc.MapNpc.Id;
                    break;

                case IMonsterEntity monster:
                    su.TargetVisualType = VisualType.Monster;
                    su.TargetId = monster.MapMonster.Id;
                    break;
            }

            return su;
        }
    }
}