using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Enums.Game.Portals;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Game.Maps;

namespace ChickenAPI.Game.Game.Components
{
    public class PortalComponent : IComponent
    {
        public PortalComponent(IEntity entity, PortalDto dto)
        {
            Entity = entity;
            PortalId = dto.Id;
            Type = dto.Type;
            DestinationMapLayer = null;
            DestinationX = dto.DestinationX;
            DestinationY = dto.DestinationY;
            SourceX = dto.SourceX;
            SourceY = dto.SourceY;
            IsDisabled = dto.IsDisabled;
        }

        public IEntity Entity { get; }

        public long PortalId { get; set; }

        public PortalType Type { get; set; }

        public IMapLayer DestinationMapLayer { get; set; }

        public short DestinationX { get; set; }
        public short DestinationY { get; set; }

        public short SourceX { get; set; }
        public short SourceY { get; set; }
        public bool IsDisabled { get; set; }
    }
}