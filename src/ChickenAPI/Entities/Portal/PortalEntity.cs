using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Game.Components;

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