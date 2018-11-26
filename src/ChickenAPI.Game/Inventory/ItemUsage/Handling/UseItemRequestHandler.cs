using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Args;
using ChickenAPI.Game.Permissions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ChickenAPI.Game.Inventory.ItemUsage.Handling
{
    public class UseItemRequestHandler
    {
        private readonly IEnumerable<PermissionsRequirementsAttribute> _permissions;
        private readonly Action<IPlayerEntity, InventoryUseItemEvent> _func;

        public UseItemRequestHandler(MethodInfo method) : this(method.GetCustomAttribute<UseItemEffectAttribute>(), method)
        {
        }

        public UseItemRequestHandler(UseItemEffectAttribute attribute, MethodInfo method)
        {
            //Item = attribute.Item;
            Effect = attribute.EffectId;
            IType = attribute.IType;

            if (method == null)
            {
                throw new Exception($"[UI] Your handler for {Effect} is wrong");
            }

            _permissions = method.GetCustomAttributes<PermissionsRequirementsAttribute>();
            _func = (Action<IPlayerEntity, InventoryUseItemEvent>)Delegate.CreateDelegate(typeof(Action<IPlayerEntity, InventoryUseItemEvent>), method);
        }

        public void Handle(IPlayerEntity player, InventoryUseItemEvent e)
        {
            /* if (!_permissions.All(player.HasPermission))
            {
                return;
            }
            */

            _func.Invoke(player, e);
        }

        public long Effect { get; }

        public ItemType IType { get; }
    }
}