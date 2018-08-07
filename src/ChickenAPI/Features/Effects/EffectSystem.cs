using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;

namespace ChickenAPI.Game.Features.Effects
{
    public class EffectSystem : NotifiableSystemBase
    {
        public EffectSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            switch (e)
            {
                default:
                    return;
            }
        }
    }
}