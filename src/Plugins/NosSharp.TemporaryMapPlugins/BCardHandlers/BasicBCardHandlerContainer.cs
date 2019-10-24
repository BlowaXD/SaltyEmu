using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Enums.Game.BCard;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.BCards;

namespace SaltyEmu.BasicPlugin.BCardHandlers
{
    public class BasicBCardHandlerContainer : IBCardHandlerContainer
    {
        private readonly ILogger _log;
        protected readonly Dictionary<BCardType, IBCardEffectHandler> Useitem = new Dictionary<BCardType, IBCardEffectHandler>();

        public BasicBCardHandlerContainer(ILogger log)
        {
            _log = log;
            Assembly currentAsm = Assembly.GetAssembly(typeof(BasicPlugin));
            // get types
            foreach (Type type in currentAsm.GetTypes().Where(s => s.GetMethods().Any(m => m.GetCustomAttribute<BCardEffectHandlerAttribute>() != null)))
            {
                // each method for a type
                foreach (MethodInfo method in type.GetMethods().Where(s => s.GetCustomAttribute<BCardEffectHandlerAttribute>() != null))
                {
                    Register(new BasicBCardHandler(method));
                }
            }
        }

        public void Register(IBCardEffectHandler handler)
        {
            if (Useitem.ContainsKey(handler.HandledType))
            {
                return;
            }

            Useitem.Add(handler.HandledType, handler);
            _log.Info($"[REGISTER_HANDLER] BCARD_TYPE : {handler.HandledType} REGISTERED !");
        }

        public async Task Handle(IBattleEntity target, IBattleEntity sender, BCardDto bcard)
        {
            if (target == null)
            {
                return;
            }

            _log.Info($"Trying to cast Bcard id : {bcard.Id} : {bcard.Type}");

            if (!Useitem.TryGetValue(bcard.Type, out IBCardEffectHandler handler))
            {
                return;
            }

            await handler.Handle(target, sender, bcard);
        }
    }
}