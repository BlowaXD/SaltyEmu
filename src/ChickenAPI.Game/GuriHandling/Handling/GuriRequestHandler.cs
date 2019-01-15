using System;
using System.Reflection;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.GuriHandling.Events;

namespace ChickenAPI.Game.GuriHandling.Handling
{
    public class GuriRequestHandler
    {
        private readonly Action<IPlayerEntity, GuriEvent> _func;

        public GuriRequestHandler(MethodInfo method) : this(method.GetCustomAttribute<GuriEffectAttribute>(), method)
        {
        }

        public GuriRequestHandler(GuriEffectAttribute attribute, MethodInfo method)
        {
            GuriEffectId = attribute.EffectId;

            if (method == null)
            {
                throw new Exception($"[GURI] Your handler for {GuriEffectId} is wrong");
            }

            _func = (Action<IPlayerEntity, GuriEvent>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, GuriEvent>), method);
        }

        public long GuriEffectId { get; }

        public void Handle(IPlayerEntity player, GuriEvent e)
        {
            _func(player, e);
        }
    }
}