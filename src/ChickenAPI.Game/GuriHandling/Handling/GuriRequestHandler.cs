using System;
using System.Collections.Generic;
using System.Reflection;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.GuriHandling.Args;

namespace ChickenAPI.Game.GuriHandling.Handling
{
    public class GuriRequestHandler
    {
        private readonly Action<IPlayerEntity, GuriEventArgs> _func;

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
            
            _func = (Action<IPlayerEntity, GuriEventArgs>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, GuriEventArgs>), method);
        }

        public void Handle(IPlayerEntity player, GuriEventArgs e)
        {
            _func.Invoke(player, e);
        }

        public long GuriEffectId { get; }
    }
}