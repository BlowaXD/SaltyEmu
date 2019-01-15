using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game._ECS.Components
{
    /// <summary>
    ///     Define a component that is attached to an entity
    /// </summary>
    public interface IComponent
    {
        IEntity Entity { get; }
    }
}