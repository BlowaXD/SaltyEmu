using System;
using System.Reflection;
using System.Threading.Tasks;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;

namespace ChickenAPI.Game.Inventory.ItemUsage.Handling
{
    public class UseItemRequestHandler
    {
        private readonly Action<IPlayerEntity, InventoryUseItemEvent> _func;

        public UseItemRequestHandler(MethodInfo method) : this(method.GetCustomAttribute<UseItemEffectAttribute>(), method)
        {
        }

        public UseItemRequestHandler(UseItemEffectAttribute attribute, MethodInfo method)
        {
            Effect = attribute.EffectId;
            IType = attribute.IType;

            if (method == null)
            {
                throw new Exception($"[UI] Your handler for {Effect} is wrong");
            }

            _func = (Action<IPlayerEntity, InventoryUseItemEvent>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, InventoryUseItemEvent>), method);
        }

        public long Effect { get; }

        public ItemType IType { get; }

        public Task Handle(IPlayerEntity player, InventoryUseItemEvent e)
        {
            _func(player, e);
            return Task.CompletedTask;
        }
    }
}