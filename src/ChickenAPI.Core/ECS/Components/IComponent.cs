using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Core.ECS.Components
{
    /// <summary>
    ///     Define a component that is attached to an entity
    /// </summary>
    public interface IComponent
    {
        IEntity Entity { get; }
    }
}