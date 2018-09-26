using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Features.Effects.Args;

namespace ChickenAPI.Game.Features.Effects
{
    public class EffectEventHandler : EventHandlerBase
    {
        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case AddEffectArgument addEffect:
                    AddEffectArgument(entity, addEffect);
                    break;
            }
        }

        private static void AddEffectArgument(IEntity entity, AddEffectArgument args)
        {
            var effects = entity.GetComponent<EffectComponent>();

            if (effects == null)
            {
                effects = new EffectComponent(entity);
                entity.AddComponent(effects);
            }

            effects.Effects.Add(new EffectComponent.Effect(args.EffectId, args.Cooldown));
        }
    }
}