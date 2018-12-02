using System;
using System.Reflection;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;

namespace ChickenAPI.Game.NpcDialog.Handlers
{
    public class NpcDialogHandler
    {
        private readonly Action<IPlayerEntity, NpcDialogEventArgs> _func;

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
            
            _func = (Action<IPlayerEntity, NpcDialogEventArgs>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, NpcDialogEventArgs>), method);
        }

        public long DialogId { get; }

        public void Handle(IPlayerEntity player, NpcDialogEventArgs e)
        {
            if (e.DialogId != DialogId)
            {
                return;
            }

            _func.Invoke(player, e);
        }
    }
}