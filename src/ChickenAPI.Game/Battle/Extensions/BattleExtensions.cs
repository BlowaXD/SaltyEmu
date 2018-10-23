using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Enums.Game.Skill;
using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Features.Skills.Args;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Packets.Game.Server.Battle;
using ChickenAPI.Packets.Game.Server.QuickList.Battle;
using NLog.Targets;
using ChickenAPI.Game.Battle.Events;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class BattleExtensions
    {
        private static readonly IHitRequestFactory HitRequestFactory = new Lazy<IHitRequestFactory>(() => ChickenContainer.Instance.Resolve<IHitRequestFactory>()).Value;

        public static HitRequest CreateHitRequest(this IBattleEntity entity, IBattleEntity target, SkillDto skill)
        {
            return HitRequestFactory.CreateHitRequest(entity, target, skill);
        }

        public static void ProcessHitRequest(this IBattleEntity entity, HitRequest hit)
        {
            if (hit.Target != entity)
            {
                // this should never be here
                return;
            }

            BattleComponent battle = entity.Battle;
            uint damage = hit.Damages;
            List<SuPacket> packets = new List<SuPacket>();
            while (damage > 0)
            {
                hit.Damages = damage > ushort.MaxValue ? ushort.MaxValue : (ushort)damage;
                damage -= hit.Damages;
                packets.Add(entity.GenerateSuPacket(hit));
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

        public static void TargetHit(this IBattleEntity entity, TargetHitRequest e)
        {
            IBattleEntity target = e.Target;
            SkillComponent skillComponent = entity.Skills;
            MovableComponent movableComponent = entity.Movable;
            MovableComponent targetMovableComponent = target.Movable;
            SkillDto skill = e.Skill;

            if (!(entity is IPlayerEntity player))
            {
                player = null;
            }

            switch ((SkillTargetType)skill.TargetType)
            {
                case SkillTargetType.SingleHit:
                case SkillTargetType.SingleBuff when skill.HitType == 0:
                    if (movableComponent.GetDistance(targetMovableComponent) > skill.Range + target.Battle.BasicArea + 1)
                    {
                        goto default;
                    }
                    entity.EmitEvent(new UseSkillArgs { Skill = skill, Target = target });
                    break;

                case SkillTargetType.AOE when skill.HitType == 1: // Target Hit
                    entity.EmitEvent(new UseSkillArgs { Skill = skill, Target = target });
                    if (skill.TargetRange == 0)
                    {
                        goto default;
                    }
                    break;

                case SkillTargetType.AOE when skill.HitType != 1: // Buff
                    entity.EmitEvent(new UseSkillArgs { Skill = skill, Target = entity });
                    switch (skill.HitType)
                    {
                        case 0:
                        case 4: // Apply Buff on himself
                            break;

                        case 2: // Apply Buff in range
                            if (skill.TargetRange == 0)
                            {
                                player?.SendPacket(target.GenerateTargetCancelPacket(CancelPacketType.InCombatMode));
                                return;
                            }

                            // apply buff on each entities of type
                            break;
                    }
                    break;

                default:
                    player?.SendPacket(target.GenerateTargetCancelPacket(CancelPacketType.InCombatMode));
                    return;
            }
        }
    }
}