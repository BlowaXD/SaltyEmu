using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;

namespace NosSharp.BasicAlgorithm
{
    public class DamageAlgorithm : IDamageAlgorithm
    {
        public uint GenerateDamage(HitRequest hit)
        {
            return 100;
        }
    }
}
