using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems.Args;

namespace ChickenAPI.Core.ECS.Systems
{
    /// <summary>
    /// </summary>
    public interface INotifiableSystem : ISystem
    {
        void Execute(IEntity entity, SystemEventArgs e);
    }
}