using System;
using System.Reflection;
using ChickenAPI.Data.BCard;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.BCards
{
    public class BasicBCardHandler : IBCardEffectHandler
    {
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

        public void Handle(IBattleEntity target, IBattleEntity sender, BCardDto bcard)
        {
            _func(target, sender, bcard);
        }
    }
}