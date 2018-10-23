using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class BattleEntityStatsManagement
    {
        public static void DecreaseMp(this IBattleEntity entity, int mp)
        {
            entity.Mp -= mp;
        }
    }
}