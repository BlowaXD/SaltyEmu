using System;
using System.Collections.Generic;
using ChickenAPI.Data.Map;
using ChickenAPI.Game.Portals;
using ChickenAPI.Game.Visibility;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Enumerations;

namespace ChickenAPI.Game.Entities.Portal
{
    public class PortalEntity : EntityBase, IPortalEntity
    {
        public PortalEntity(PortalDto portal) : base(VisualType.Object, portal.Id)
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