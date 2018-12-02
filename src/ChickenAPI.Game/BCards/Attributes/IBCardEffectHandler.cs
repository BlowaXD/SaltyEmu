﻿using ChickenAPI.Data.BCard;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.BCards.Attributes
{
    public interface IBCardEffectHandler
    {
        BCardType HandledType { get; }

        void Handle(IBattleEntity target, IBattleEntity sender, BCardDto bcard);
    }
}