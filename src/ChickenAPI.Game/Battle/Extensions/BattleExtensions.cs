using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Features.Skills.Args;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Packets.Game.Server.Battle;
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
            var battle = entity.Battle;
            uint damage = hit.Damages;
            var packets = new List<SuPacket>();
            while (damage > 0)
            {
                hit.Damages = damage > ushort.MaxValue ? ushort.MaxValue : (ushort)damage;
                damage -= hit.Damages;
                packets.Add(SuPacketExtensions.GenerateSuPacket(hit));
                if (battle.Hp - hit.Damages <= 0)
                {
                    battle.Hp = 0;
                    // Generate Death
                    break;
                }
                battle.Hp -= (ushort)hit.Damages;
            }
            if (entity.CurrentMap is IMapLayer broadcastable)
            {
                broadcastable.Broadcast<SuPacket>(packets);
            }
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
                    entity.Battle.Entity.EmitEvent(new UseSkillArgs { Skill = skill, targetEntity = target });
                    break;

                // AOE Target Hit
                case 1 when skill.HitType == 1:
                    entity.Battle.Entity.EmitEvent(new UseSkillArgs { Skill = skill, targetEntity = target });
                    if (!(entity.CurrentMap is IMapLayer map) || skill.TargetRange == 0)
                    {
                        goto default;
                    }
                    break;

                // AOE Buff
                case 1 when skill.HitType != 1:
                    entity.Battle.Entity.EmitEvent(new UseSkillArgs { Skill = skill, targetEntity = entity });
                    switch (skill.HitType)
                    {
                        case 0:
                        case 4: // Apply Buff on himself
                            break;

                        case 2: // Apply Buff in range
                            if (!(entity.CurrentMap is IMapLayer mapl) || skill.TargetRange == 0)
                            {
                                player?.SendPacket(new CancelPacket { Type = CancelPacketType.InCombatMode, TargetId = target.Battle.Entity.Id });
                                return;
                            }
                            // apply buff on each entities of type
                            break;
                    }
                    break;

                // Buff
                case 2 when skill.HitType == 0:
                    entity.Battle.Entity.EmitEvent(new UseSkillArgs { Skill = skill, targetEntity = entity });
                    break;

                default:
                    player?.SendPacket(new CancelPacket { Type = CancelPacketType.InCombatMode, TargetId = target.Battle.Entity.Id });
                    return;
            }
        }
    }
}