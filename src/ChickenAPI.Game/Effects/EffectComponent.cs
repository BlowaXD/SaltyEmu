using System;
using System.Collections.Generic;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Effects
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