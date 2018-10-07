using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Features.Skills.Args;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Packets.Game.Server.QuickList.Battle;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class BattleExtensions
    {
        private static readonly IHitRequestFactory _HitRequestFactory = new Lazy<IHitRequestFactory>(() => ChickenContainer.Instance.Resolve<IHitRequestFactory>()).Value;

        public static Hitting.HitRequest CreateHitRequest(this IBattleEntity entity, IBattleEntity target)
        {
            return _HitRequestFactory.CreateHitRequest(entity, target);
        }

        public static void ProcessHitRequest(this IBattleEntity entity, Hitting.HitRequest hit)
        {
            if (hit.Target != entity)
            {
                // this should never be here
                return;
            }

            // remove hp
            // apply buffs
            // apply debuffs
        }

        public static void TargetHit(this IBattleEntity entity, IBattleEntity target, long skillId)
        {
            var skillComponent = entity.Battle.Entity.GetComponent<SkillComponent>();
            var movableComponent = entity.Battle.Entity.GetComponent<MovableComponent>();
            var targetMovableComponent = entity.Battle.Entity.GetComponent<MovableComponent>();
            if (!(entity.Battle.Entity is IPlayerEntity player))
            {
                player = null;
            }

            SkillDto skill = skillComponent.Skills[skillId];
            if (skill == null || SkillEventHandler.TryCastSkill(skillComponent, new SkillCastArgs { Skill = skill }))
            {
                player?.SendPacket(new CancelPacket { Type = CancelPacketType.InCombatMode, TargetId = target.Battle.Entity.Id });
                return;
            }

            switch (skill.TargetType)
            {
                // Single Hit
                case 0:
                    if (movableComponent.GetDistance(targetMovableComponent) > skill.Range + target.Battle.BasicArea + 1)
                    {
                        goto default;
                    }

                    entity.Battle.Entity.EmitEvent(new UseSkillArgs { Skill = skill });

                    break;

                // AOE Target Hit
                case 1 when skill.HitType == 1:
                    break;

                // AOE Buff
                case 1 when skill.HitType != 1:
                    break;

                // Buff
                case 2 when skill.HitType == 0:
                    break;

                default:
                    player?.SendPacket(new CancelPacket { Type = CancelPacketType.InCombatMode, TargetId = target.Battle.Entity.Id });
                    return;
            }
        }
    }
}