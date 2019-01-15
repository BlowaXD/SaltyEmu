using ChickenAPI.Game.Portals;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Entities.Portal
{
    /// <summary>
    ///     Portals
    /// </summary>
    public interface IPortalEntity : IEntity
    {
        PortalComponent Portal { get; }
    }
}