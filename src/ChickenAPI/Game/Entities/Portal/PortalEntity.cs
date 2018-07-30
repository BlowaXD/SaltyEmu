using System;
using System.Collections.Generic;
using ChickenAPI.Data.TransferObjects.Map;
using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;

namespace ChickenAPI.Game.Entities.Portal
{
    public class PortalEntity : EntityBase
    {
        public PortalEntity(PortalDto portal) : base(EntityType.Portal)
        {
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(PortalComponent), new PortalComponent(this, portal) }
            };
        }

        public override void Dispose()
        {
        }
    }
}