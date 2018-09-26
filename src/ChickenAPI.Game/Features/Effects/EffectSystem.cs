using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Game.Maps;
using ChickenAPI.Packets.Game.Client._NotYetSorted;

namespace ChickenAPI.Game.Features.Effects
{
    public class EffectSystem : SystemBase
    {
        public EffectSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        /// <summary>
        ///     Once per second
        /// </summary>
        protected override double RefreshRate => 1;

        protected override Expression<Func<IEntity, bool>> Filter => entity => entity.HasComponent<EffectComponent>();


        protected override void Execute(IEntity entity)
        {
            List<EffectPacket> packets = new List<EffectPacket>();
            var effects = entity.GetComponent<EffectComponent>();
            foreach (EffectComponent.Effect effect in effects.Effects)
            {
                if (effect.LastCast.AddMilliseconds(effect.Cooldown) > DateTime.UtcNow)
                {
                    continue;
                }

                effect.LastCast = DateTime.UtcNow;
                packets.Add(entity.GenerateEffectPacket(effect.Id));
            }


            if (entity.EntityManager is IMapLayer mapLayer)
            {
                mapLayer.Broadcast((IEnumerable<EffectPacket>)packets);
            }
        }
    }
}