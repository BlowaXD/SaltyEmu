using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChickenAPI.Game.Inventory.ItemUsage.Handling
{
    public class BaseUseItemHandler : IItemUsageContainer
    {
        private static readonly Logger Log = Logger.GetLogger<BaseUseItemHandler>();

        protected readonly Dictionary<Tuple<long, ItemType>, UseItemRequestHandler> useitem;

        public BaseUseItemHandler()
        {
            useitem = new Dictionary<Tuple<long, ItemType>, UseItemRequestHandler>();

            Assembly currentAsm = Assembly.GetAssembly(typeof(BaseUseItemHandler));
            // get types
            foreach (Type type in currentAsm.GetTypes().Where(s => s.GetMethods().Any(m => m.GetCustomAttribute<UseItemEffectAttribute>() != null)))
            {
                // each method for a type
                foreach (MethodInfo method in type.GetMethods().Where(s => s.GetCustomAttribute<UseItemEffectAttribute>() != null))
                {
                    RegisterItemUsageCallback(new UseItemRequestHandler(method));
                }
            }
        }

        public void RegisterItemUsageCallback(UseItemRequestHandler handler)
        {
            if (useitem.ContainsKey(Tuple.Create(handler.Effect, handler.IType)))
            {
                return;
            }

            useitem.Add(Tuple.Create(handler.Effect, handler.IType), handler);
            Log.Info($"[REGISTER_HANDLER] UI_EFFECT : {handler.Effect} && ITYPE : {handler.IType} REGISTERED !");
        }

        public void UseItem(IPlayerEntity player, InventoryUseItemEvent args)
        {
            if (player == null)
            {
                return;
            }

            if (!useitem.TryGetValue(Tuple.Create((long)args.Item.Item.Effect, args.Item.Item.ItemType), out UseItemRequestHandler handler))
            {
                return;
            }

            handler.Handle(player, args);
        }
    }
}