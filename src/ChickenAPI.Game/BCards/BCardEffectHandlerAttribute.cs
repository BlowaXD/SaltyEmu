using System;
using ChickenAPI.Enums.Game.BCard;

namespace ChickenAPI.Game.BCards.Attributes
{
    public class BCardEffectHandlerAttribute : Attribute
    {
        public BCardEffectHandlerAttribute(BCardType cardType) => CardType = cardType;

        public BCardType CardType { get; }
    }
}