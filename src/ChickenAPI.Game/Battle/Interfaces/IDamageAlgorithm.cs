using ChickenAPI.Game.Battle.Hitting;

namespace ChickenAPI.Game.Battle.Interfaces
{
    public interface IDamageAlgorithm
    {
        /// <summary>
        /// Returns the damages when one entity strikes another based on their stats
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="targetEntity"></param>
        /// <returns></returns>
        uint GenerateDamage(HitRequest hit);
    }
}
