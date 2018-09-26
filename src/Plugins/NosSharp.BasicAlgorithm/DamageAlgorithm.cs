using ChickenAPI.Game.Data.AccessLayer.Battle;
using ChickenAPI.Game.Entities;

namespace NosSharp.BasicAlgorithm
{
    public class DamageAlgorithm : IDamageAlgorithm
    {
        public ushort GenerateDamage(IBattleEntity entity, IBattleEntity targetEntity)
        {
            return 100;
        }
    }
}
