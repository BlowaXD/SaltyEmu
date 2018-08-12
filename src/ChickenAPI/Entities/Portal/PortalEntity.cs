using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Features.Portals;
using ChickenAPI.Game.Features.Visibility;

namespace ChickenAPI.Game.Entities.Portal
{
    public class PortalEntity : EntityBase
    {
        public PortalEntity(PortalDto portal) : base(EntityType.Portal)
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