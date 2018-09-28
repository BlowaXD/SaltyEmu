using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Features.Portals;

namespace ChickenAPI.Game.Entities.Portal
{
    /// <summary>
    /// Portals
    /// </summary>
    public interface IPortalEntity : IEntity
    {
        PortalComponent Portal { get; }
    }
}