using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class TargetExtensions
    {
        public static void SetTarget(this IBattleEntity entity, IBattleEntity target)
        {
            entity.Target = target;
        }
    }
}