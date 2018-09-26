using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Game.Features.Effects
{
    public class EffectComponent : IComponent
    {
        public EffectComponent(IEntity entity)
        {
            Entity = entity;
            Effects = new List<Effect>();
        }

        public List<Effect> Effects { get; set; }

        public IEntity Entity { get; }

        public class Effect
        {
            public Effect(long id, long cooldown)
            {
                Id = id;
                Cooldown = cooldown;
                LastCast = DateTime.MinValue;
            }

            public long Id { get; }

            /// <summary>
            ///     In milliseconds
            /// </summary>
            public long Cooldown { get; }

            public DateTime LastCast { get; set; }
        }
    }
}