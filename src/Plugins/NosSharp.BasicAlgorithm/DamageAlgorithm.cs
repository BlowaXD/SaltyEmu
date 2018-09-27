using ChickenAPI.Game.Battle;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities;

namespace NosSharp.BasicAlgorithm
{
    public class DamageAlgorithm : IDamageAlgorithm
    {
        public uint GenerateDamage(IBattleEntity entity, IBattleEntity targetEntity)
        {
            return 100;
        }
    }
}
