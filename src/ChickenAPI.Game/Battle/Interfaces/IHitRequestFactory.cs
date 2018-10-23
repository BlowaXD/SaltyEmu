using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Battle.Hitting;

namespace ChickenAPI.Game.Battle.Interfaces
{
    public interface IHitRequestFactory
    {
        /// <summary>
        /// Creates an hit request
        /// This centralized version will permit you to apply some "default properties" to your hit requests
        /// For example, having a % of lifesteal in each hit, depending on your wills
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        HitRequest CreateHitRequest(IBattleEntity sender, IBattleEntity target, SkillDto skill);
    }
}