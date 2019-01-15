using System;
using System.Reflection;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;

namespace ChickenAPI.Game.NpcDialog.Handling
{
    public class NpcDialogHandler
    {
        private readonly Action<IPlayerEntity, NpcDialogEvent> _func;

        public NpcDialogHandler(MethodInfo method) : this(method.GetCustomAttribute<NpcDialogHandlerAttribute>(), method)
        {
        }

        public NpcDialogHandler(NpcDialogHandlerAttribute attribute, MethodInfo method)
        {
            DialogId = attribute.NpcDialogId;

            if (method == null)
            {
                throw new Exception($"Your handler for {DialogId} is wrong");
            }

            _func = (Action<IPlayerEntity, NpcDialogEvent>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, NpcDialogEvent>), method);
        }

        public long DialogId { get; }

        public void Handle(IPlayerEntity player, NpcDialogEvent e)
        {
            if (e.DialogId != DialogId)
            {
                return;
            }

            _func.Invoke(player, e);
        }
    }
}