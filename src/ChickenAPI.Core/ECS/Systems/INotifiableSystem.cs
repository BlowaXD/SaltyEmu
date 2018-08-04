using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Core.ECS.Systems
{
    /// <summary>
    /// </summary>
    public interface INotifiableSystem : ISystem
    {
        void Execute(IEntity entity, SystemEventArgs e);
    }
}