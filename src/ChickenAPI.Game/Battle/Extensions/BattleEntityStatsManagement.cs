using System.Threading.Tasks;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class BattleEntityStatsManagement
    {
        private static Task CheckEntity(IBattleEntity entity)
        {
            if (entity is IPlayerEntity session)
            {
                return session.ActualizeUiHpBar();
            }
            return Task.CompletedTask;
        }

        public static Task DecreaseMp(this IBattleEntity entity, int mp)
        {
            entity.Mp -= mp;
            return CheckEntity(entity);
        }

        public static Task DecreaseHp(this IBattleEntity entity, int hp)
        {
            entity.Hp -= hp;
            return CheckEntity(entity);
        }

        public static Task IncreaseMp(this IBattleEntity entity, int mp)
        {
            entity.Mp += mp;
            return CheckEntity(entity);
        }

        public static Task IncreaseHp(this IBattleEntity entity, int hp)
        {
            entity.Hp += hp;
            return CheckEntity(entity);
        }
    }
}