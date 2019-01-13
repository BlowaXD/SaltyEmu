using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.BCards.Attributes;
using ChickenAPI.Game.Buffs;

namespace SaltyEmu.BasicPlugin.BCardHandlers
{
    public static class BCardBuffHandler
    {
        [BCardEffectHandler(BCardType.Buff)]
        public static void Handle(IBattleEntity target, IBattleEntity sender, BCardDto bcard)
        {
            var random = ChickenContainer.Instance.Resolve<IRandomGenerator>();
            if (random.Next() >= bcard.FirstData)
            {
                return;
            }

            CardDto card = ChickenContainer.Instance.Resolve<ICardService>().GetById(bcard.SecondData);

            if (card == null)
            {
                return;
            }

            target.AddBuff(new BuffContainer(card, sender.Level));
        }
    }
}