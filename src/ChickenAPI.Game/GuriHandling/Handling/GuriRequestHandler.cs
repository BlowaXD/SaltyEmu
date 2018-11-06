using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.GuriHandling.Args;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Game.Features.GuriHandling.Handling
{
    public class GuriRequestHandler
    {
        private readonly IEnumerable<PermissionsRequirementsAttribute> _permissions;
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

            _permissions = method.GetCustomAttributes<PermissionsRequirementsAttribute>();
            _func = (Action<IPlayerEntity, GuriEventArgs>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, GuriEventArgs>), method);
        }

        public void Handle(IPlayerEntity player, GuriEventArgs e)
        {
            /* if (!_permissions.All(player.HasPermission))
            {
                return;
            }
            */

            _func.Invoke(player, e);
        }

        public long GuriEffectId { get; }
    }
}