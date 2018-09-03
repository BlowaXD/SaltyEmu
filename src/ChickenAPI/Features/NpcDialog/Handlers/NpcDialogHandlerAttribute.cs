using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;

namespace ChickenAPI.Game.Features.NpcDialog.Handlers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NpcDialogHandlerAttribute : Attribute
    {
        private readonly Action<IPlayerEntity, NpcDialogEventArgs> _func;

        public NpcDialogHandlerAttribute(long npcDialogId, Type type)
        {
            if (type.BaseType != typeof(Delegate) && type.BaseType != typeof(MulticastDelegate) && type != typeof(NpcDialogDelegate))
            {
                throw new Exception("Can only accept delegates");
            }

            MethodInfo method = type.GetMethods().FirstOrDefault(s => s.GetCustomAttribute<NpcDialogHandlerAttribute>()?.NpcDialogId == npcDialogId);
            if (method == null)
            {
                throw new Exception($"Your handler for {npcDialogId} is wrong");
            }

            _func = (Action<IPlayerEntity, NpcDialogEventArgs>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, NpcDialogEventArgs>), method);
            NpcDialogId = npcDialogId;
        }

        public long NpcDialogId { get; }

        public void Handle(IPlayerEntity player, NpcDialogEventArgs eventArgs)
        {
            _func.Invoke(player, eventArgs);
        }
    }
}