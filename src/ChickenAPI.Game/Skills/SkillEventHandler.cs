using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Skill;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.Skills.Args;
using ChickenAPI.Game.Movements.Extensions;

namespace ChickenAPI.Game.Skills
{
    public class SkillEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(UseSkillArgs), typeof(PlayerAddSkillEventArgs)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            if (!(entity is IBattleEntity battleEntity))
            {
                return;
            }

            switch (e)
            {
                case UseSkillArgs useSkill:
                    UseSkill(battleEntity, useSkill);
                    break;
                case PlayerAddSkillEventArgs addSkill:
                    AddSkill(entity as IPlayerEntity, addSkill);
                    break;
            }
        }

        public static void UseSkill(IBattleEntity entity, UseSkillArgs e)
        {
            IBattleEntity target = e.Target;
            SkillDto skill = e.Skill;

            if (!(entity is IPlayerEntity player))
            {
                player = null;
            }

            if (e.Skill.MpCost > entity.Mp) //TODO: others check
            {
                player?.SendPacket(e.Target.GenerateTargetCancelPacket(CancelPacketType.NotInCombatMode));
                return;
            }

            List<IBattleEntity> targets = new List<IBattleEntity>();
            switch ((SkillTargetType)skill.TargetType)
            {
                case SkillTargetType.SingleHit:
                case SkillTargetType.SingleBuff when skill.HitType == 0:
                    if (entity.GetDistance(target) > skill.Range + target.BasicArea + 1)
                    {
                        goto default;
                    }

                    targets.Add(target);
                    break;

                case SkillTargetType.AOE when skill.HitType == 1: // Target Hit
                    if (skill.TargetRange == 0)
                    {
                        goto default;
                    }

                    break;

                case SkillTargetType.AOE when skill.HitType != 1: // Buff
                    switch (skill.HitType)
                    {
                        case 0:
                        case 4: // Apply Buff on himself
                            break;

                        case 2: // Apply Buff in range
                            if (skill.TargetRange == 0)
                            {
                                player?.SendPacket(target.GenerateTargetCancelPacket(CancelPacketType.NotInCombatMode));
                                return;
                            }

                            // apply buff on each entities of type
                            break;
                    }

                    break;

                default:
                    player?.SendPacket(target.GenerateTargetCancelPacket(CancelPacketType.NotInCombatMode));
                    return;
            }


            entity.CurrentMap.Broadcast(entity.GenerateCtPacket(e.Target, e.Skill));
            entity.DecreaseMp(e.Skill.MpCost);
            //TODO: Skill Cooldown

            targets.ForEach(t =>
            {
                HitRequest hitRequest = entity.CreateHitRequest(t, e.Skill);

                entity.EmitEvent(new FillHitRequestEvent
                {
                    HitRequest = hitRequest,
                });

                entity.EmitEvent(new ProcessHitRequestEvent
                {
                    HitRequest = hitRequest
                });
            });
        }

        /// <summary>
        /// Todo rework
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        public static void AddSkill(IPlayerEntity player, PlayerAddSkillEventArgs e)
        {
            SkillComponent component = player.SkillComponent;
            if (e.Skill is null)
            {
                return; //the skill doesn't exist?
            }

            if (e.ForceChecks)
            {
                CharacterDto character = player.Character;

                if (character is null)
                {
                    return; //we need it.
                }

                if (e.Skill.CpCost > 0)
                {
                    return; //not enough cp to learn that skill.
                }

                if (e.Skill.Price > int.MaxValue - 1) //we need to get entity's gold count.
                {
                    return; //not enough gold to learn that skill.
                }

                if (e.Skill.LevelMinimum > player.JobLevel)
                {
                    return; //the joblevel of the entity isn't high enough.
                }

                if (e.Skill.Class != (byte)character.Class)
                {
                    return; //the class of the entity doesn't correspond of the skill requirements.
                }

                int classLevel = 0;
                switch (character.Class)
                {
                    case CharacterClassType.Adventurer:
                        classLevel = e.Skill.MinimumAdventurerLevel;
                        break;
                    case CharacterClassType.Swordman:
                        classLevel = e.Skill.MinimumSwordmanLevel;
                        break;
                    case CharacterClassType.Archer:
                        classLevel = e.Skill.MinimumArcherLevel;
                        break;
                    case CharacterClassType.Magician:
                        classLevel = e.Skill.MinimumMagicianLevel;
                        break;
                    case CharacterClassType.Wrestler:
                        classLevel = e.Skill.MinimumWrestlerLevel;
                        break;
                    case CharacterClassType.Unknown:
                        classLevel = e.Skill.LevelMinimum;
                        break;
                }

                if (classLevel > player.Level)
                {
                    return; //the level of the entity isn't high enough.
                }
            }

            if (e.Skill.Id < 200)
            {
                foreach (SkillDto skill in component.Skills.Values)
                {
                    if (e.Skill.CastId == skill.CastId && skill.Id < 200)
                    {
                        component.Skills.Remove(skill.Id);
                    }
                }
            }
            else
            {
                if (component.Skills.ContainsKey(e.Skill.Id))
                {
                    return; //we already have that skill!
                }

                if (e.Skill.UpgradeSkill != 0) //means it's not a skill but an upgrade
                {
                    SkillDto oldUpgrade = component.Skills.Values.FirstOrDefault(
                        s => s.UpgradeSkill == e.Skill.UpgradeSkill &&
                            s.UpgradeType == e.Skill.UpgradeType &&
                            s.UpgradeSkill != 0);

                    if (!(oldUpgrade is null))
                    {
                        component.Skills.Remove(oldUpgrade.Id);
                    }
                }
            }

            if (!component.Skills.ContainsKey(e.Skill.Id))
            {
                component.Skills.Add(e.Skill.Id, e.Skill);
            }

            //todo: send different packets to add the skill.
        }
    }
}