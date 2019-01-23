using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Skill;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Movements;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.Skills.Args;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game._ECS.Systems;

namespace ChickenAPI.Game.IAs
{
    public class IASystem : SystemBase
    {
        private readonly IMap _map;
        private readonly IPathfinder _pathfinder;
        private readonly IRandomGenerator _random;

        public IASystem(IEntityManager entityManager, IMap map) : base(entityManager)
        {
            _map = map;
            _pathfinder = ChickenContainer.Instance.Resolve<IPathfinder>();
            _random = ChickenContainer.Instance.Resolve<IRandomGenerator>();
        }

        protected override double RefreshRate => 0.45;

        protected override Expression<Func<IEntity, bool>> Filter => entity => MovableFilter(entity);

        private static bool MovableFilter(IEntity entity)
        {
            if (entity.Type == VisualType.Character)
            {
                return false;
            }


            return entity is IAiEntity;
        }

        private Task FollowTheTarget(IAiEntity aiEntity, IBattleEntity target)
        {
            return Task.CompletedTask;
        }

        private Task ProcessAiWithTarget(IAiEntity aiEntity, IBattleEntity target)
        {
            // if range is enough to attack, attack 
            int distance = aiEntity.GetDistance(target);

            // get a random skill
            SkillDto skill = aiEntity.Skills.Values.OrderBy(s => _random.Next()).FirstOrDefault(s => s.Range >= distance);

            // no skill has range to hit target
            if (skill == null)
            {
                byte range = aiEntity.NpcMonster.BasicRange;

                // basic skill range
                if (distance > range)
                {
                    // follow the target
                    return FollowTheTarget(aiEntity, target);
                }

                skill = new SkillDto
                {
                    HitType = (byte)SkillTargetType.SingleHit,
                    Range = range,
                    MpCost = 0, // basic skill costs nothing
                    Cooldown = (short)(aiEntity.NpcMonster.BasicCooldown * 250),
                    TargetType = (byte)SkillTargetType.SingleHit,
                    Effect = aiEntity.NpcMonster.BasicSkill,
                    AttackAnimation = 11, // default value for SingleTargetHit
                    SkillType = 0 // default value for BasicSkill
                };
            }

            // hit the target with the given skill
            return aiEntity.EmitEventAsync(new UseSkillEvent { Skill = skill, Target = target });
        }

        //todo async systems
        protected override void Execute(IEntity entity)
        {
            if (!(entity is IAiEntity mov))
            {
                return;
            }

            if (!mov.IsAlive)
            {
                return;
            }

            if (mov.HasTarget)
            {
                ProcessAiWithTarget(mov, mov.Target).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
            {
                ProcessAiWithoutTarget(mov);
            }
        }

        private void ProcessAiWithoutTarget(IAiEntity mov)
        {
            if (mov.Waypoints != null && mov.Waypoints.Length != 0)
            {
                return;
            }
            // supposedly should never enter here
            if (mov.Type == VisualType.Character || mov.Speed == 0)
            {
                return;
            }

            if (_random.Next(0, 100) < 35)
            {
                // wait max 2500 millisecs before having a new movement
                mov.LastMove = DateTime.UtcNow.AddMilliseconds(_random.Next(2500));
                return;
            }

            int i = 0;
            Position<short> dest = null;
            while (dest == null && i < 25)
            {
                short xpoint = (short)_random.Next(0, 4);
                short ypoint = (short)_random.Next(0, 4);
                short firstX = mov.Position.X;
                short firstY = mov.Position.Y;
                dest = _map.GetFreePosition(firstX, firstY, xpoint, ypoint);

                i++;
            }

            if (dest == null)
            {
                return;
            }

            mov.Waypoints = _pathfinder.FindPath(mov.Position, dest, _map);
        }
    }
}