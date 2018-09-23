using System;
using System.Collections.Generic;
using System.Reflection;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Game.Features.NpcDialog.Handlers
{
    public class NpcDialogHandler
    {
        private readonly long _dialogId;
        private readonly IEnumerable<PermissionsRequirementsAttribute> _permissions;
        private readonly Action<IPlayerEntity, NpcDialogEventArgs> _func;

        public NpcDialogHandler(MethodInfo method) : this(method.GetCustomAttribute<NpcDialogHandlerAttribute>(), method)
        {
        }

        public NpcDialogHandler(NpcDialogHandlerAttribute attribute, MethodInfo method)
        {
            _dialogId = attribute.NpcDialogId;

            if (method == null)
            {
                throw new Exception($"Your handler for {_dialogId} is wrong");
            }

            _permissions = method.GetCustomAttributes<PermissionsRequirementsAttribute>();
            _func = (Action<IPlayerEntity, NpcDialogEventArgs>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, NpcDialogEventArgs>), method);
        }

        public long DialogId => _dialogId;

        public void Handle(IPlayerEntity player, NpcDialogEventArgs e)
        {
            if (e.DialogId != _dialogId)
            {
                return;
            }
        }
    }
}