using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Game.Features.NpcDialog.Handlers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NpcDialogHandlerAttribute : Attribute
    {
        private readonly Action<IPlayerEntity, NpcDialogEventArgs> _func;
        private readonly IEnumerable<PermissionsRequirementsAttribute> _permissions;

        public NpcDialogHandlerAttribute(long npcDialogId, Type type)
        {
            MethodInfo method = type.GetMethods().FirstOrDefault(s => s.GetCustomAttribute<NpcDialogHandlerAttribute>()?.NpcDialogId == npcDialogId);
            if (method == null)
            {
                throw new Exception($"Your handler for {npcDialogId} is wrong");
            }

            _permissions = method.GetCustomAttributes<PermissionsRequirementsAttribute>();

            _func = (Action<IPlayerEntity, NpcDialogEventArgs>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, NpcDialogEventArgs>), method);
            NpcDialogId = npcDialogId;
        }

        public long NpcDialogId { get; }

        public void Handle(IPlayerEntity player, NpcDialogEventArgs eventArgs)
        {
            if (_permissions.Any(permission => !player.HasPermission(permission)))
            {
                return;
            }

            _func.Invoke(player, eventArgs);
        }
    }
}