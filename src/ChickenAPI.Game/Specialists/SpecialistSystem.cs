﻿using System;
using System.Linq.Expressions;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.ECS.Systems;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;

namespace ChickenAPI.Game.Specialists
{
    public class SpecialistSystem : SystemBase
    {
        public SpecialistSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        // all 10 seconds
        protected override double RefreshRate => 0.1;
        protected override Expression<Func<IEntity, bool>> Filter => entity => entity.Type == VisualType.Character;

        protected override void Execute(IEntity entity)
        {
            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            if (!player.HasSpWeared)
            {
                return;
            }

            // remove points
            player.ActualiseUiSpPoints();
        }
    }
}