using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Core.ECS.Components
{
    /// <summary>
    ///     Define a component that can be
    /// </summary>
    public interface IComponent
    {
        IEntity Entity { get; }
    }
}