using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class BattleExtensions
    {
        private static readonly IHitRequestFactory HitRequestFactory = new Lazy<IHitRequestFactory>(() => ChickenContainer.Instance.Resolve<IHitRequestFactory>()).Value;

        public static HitRequest CreateHitRequest(this IBattleEntity entity, IBattleEntity target, SkillDto skill)
        {
            HitRequest request = HitRequestFactory.CreateHitRequest(entity, target, skill);

            return request;
        }
    }
}