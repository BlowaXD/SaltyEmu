using System;
using System.Collections.Generic;
using ChickenAPI.Data.Map;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Features.Portals;
using ChickenAPI.Game.Visibility;

namespace ChickenAPI.Game.Entities.Portal
{
    public class PortalEntity : EntityBase
    {
        public PortalEntity(PortalDto portal) : base(VisualType.Portal, portal.Id)
        {
            Portal = new PortalComponent(this, portal);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(PortalComponent), Portal },
                { typeof(VisibilityComponent), new VisibilityComponent(this) }
            };
        }

        public PortalComponent Portal { get; }

        public override void Dispose()
        {
        }
    }
}