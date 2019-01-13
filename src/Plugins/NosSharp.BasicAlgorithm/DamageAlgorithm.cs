using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;

namespace SaltyEmu.BasicAlgorithmPlugin
{
    public class DamageAlgorithm : IDamageAlgorithm
    {
        public uint GenerateDamage(HitRequest hit)
        {
            return 100;
        }
    }
}
