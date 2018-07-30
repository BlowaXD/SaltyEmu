using ChickenAPI.ECS.Entities;

namespace ChickenAPI.ECS.Systems
{
    /// <summary>
    /// </summary>
    public interface INotifiableSystem : ISystem
    {
        void Execute(IEntity entity, SystemEventArgs e);
    }
}