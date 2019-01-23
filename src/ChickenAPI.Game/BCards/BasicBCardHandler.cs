using System;
using System.Reflection;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.BCard;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.BCards
{
    public class BasicBCardHandler : IBCardEffectHandler
    {
        private readonly Logger Log = Logger.GetLogger<BasicBCardHandler>();
        private readonly Action<IBattleEntity, IBattleEntity, BCardDto> _func;

        public BasicBCardHandler(MethodInfo method) : this(method.GetCustomAttribute<BCardEffectHandlerAttribute>(), method)
        {
        }

        public BasicBCardHandler(BCardEffectHandlerAttribute attribute, MethodInfo method)
        {
            HandledType = attribute.CardType;

            if (method == null)
            {
                throw new Exception($"[UI] Your handler for {attribute.CardType} is wrong");
            }

            _func = (Action<IBattleEntity, IBattleEntity, BCardDto>)Delegate.CreateDelegate(typeof(Action<IBattleEntity, IBattleEntity, BCardDto>), method);
        }

        public BCardType HandledType { get; }

        public Task Handle(IBattleEntity target, IBattleEntity sender, BCardDto bcard)
        {
            Log.Info($"Handling : {HandledType}");
            _func(target, sender, bcard);
            return Task.CompletedTask;
        }
    }
}