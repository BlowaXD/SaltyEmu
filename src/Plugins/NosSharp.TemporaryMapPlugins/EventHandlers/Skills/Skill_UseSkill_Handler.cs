using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Skill;
using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.Skills.Args;

namespace SaltyEmu.BasicPlugin.EventHandlers.Skills
{
    public class Skill_UseSkill_Handler : GenericEventPostProcessorBase<UseSkillEvent>
    {
        private readonly IHitRequestFactory _hitRequestFactory;

        public Skill_UseSkill_Handler(IHitRequestFactory hitRequestFactory)
        {
            _hitRequestFactory = hitRequestFactory;
        }

        protected override async Task Handle(UseSkillEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IBattleEntity entity))
            {
                return;
            }

            IBattleEntity target = e.Target;
            SkillDto skill = e.Skill;

            if (!(entity is IPlayerEntity player))
            {
                player = null;
            }

            if (e.Skill.MpCost > entity.Mp) //TODO: others check
            {
                if (!(player is null))
                {
                    await player.SendPacketAsync(target.GenerateTargetCancelPacket(CancelPacketType.InCombatMode));
                }

                return;
            }

            List<IBattleEntity> targets = new List<IBattleEntity>();
            switch ((SkillTargetType)skill.TargetType)
            {
                case SkillTargetType.SingleHit:
                case SkillTargetType.SingleBuff when skill.HitType == 0:
                    // if too much distance, send cancel packet
                    if (entity.GetDistance(target) > skill.Range + target.BasicArea + 1)
                    {
                        if (!(player is null))
                        {
                            await player.SendPacketAsync(target.GenerateTargetCancelPacket(CancelPacketType.InCombatMode));
                        }

                        return;
                    }

                    if (entity.Type == VisualType.Player && target.Type == VisualType.Player && !entity.CurrentMap.IsPvpEnabled && skill.HitType != 1)
                    {
                        if (!(player is null))
                        {
                            await player.SendPacketAsync(target.GenerateTargetCancelPacket(CancelPacketType.InCombatMode));
                        }

                        return;
                    }

                    targets.Add(target);
                    break;

                case SkillTargetType.AOE when skill.HitType == 1: // Target Hit
                    if (skill.TargetRange == 0)
                    {
                        targets.Add(target);
                        goto default;
                    }

                    targets.Add(target);
                    break;

                case SkillTargetType.AOE when skill.HitType != 1: // Buff
                    switch (skill.HitType)
                    {
                        case 0:
                        case 4: // Apply Buff on himself
                            targets.Add(target);


                            break;

                        case 2: // Apply Buff in range
                            if (skill.TargetRange == 0)
                            {
                                if (!(player is null))
                                {
                                    await player.SendPacketAsync(target.GenerateTargetCancelPacket(CancelPacketType.InCombatMode));
                                }

                                return;
                            }
                            
                            // apply buff on each entities of type
                            break;
                    }

                    break;

                default:
                    if (!(player is null))
                    {
                        await player.SendPacketAsync(target.GenerateTargetCancelPacket(CancelPacketType.InCombatMode));
                    }
                    targets.Add(target);

                    return;
            }

            await entity.CurrentMap.BroadcastAsync(entity.GenerateCtPacket(e.Target, e.Skill));
            await entity.DecreaseMp(e.Skill.MpCost);
            //TODO: Skill Cooldown

            foreach (IBattleEntity t in targets)
            {
                HitRequest hitRequest = await _hitRequestFactory.CreateHitRequest(entity, t, e.Skill);

                await entity.EmitEventAsync(new FillHitRequestEvent
                {
                    HitRequest = hitRequest,
                });

                await entity.EmitEventAsync(new ProcessHitRequestEvent
                {
                    HitRequest = hitRequest
                });
            }
        }
    }
}