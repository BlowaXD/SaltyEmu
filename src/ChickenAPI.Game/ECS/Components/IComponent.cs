using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.ECS.Components
{
    /// <summary>
    ///     Define a component that is attached to an entity
    /// </summary>
    public interface IComponent
    {
        IEntity Entity { get; }
    }
}