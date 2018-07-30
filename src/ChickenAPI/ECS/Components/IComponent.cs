using ChickenAPI.ECS.Entities;

namespace ChickenAPI.ECS.Components
{
    /// <summary>
    ///     Define a component that can be
    /// </summary>
    public interface IComponent
    {
        IEntity Entity { get; }
    }
}