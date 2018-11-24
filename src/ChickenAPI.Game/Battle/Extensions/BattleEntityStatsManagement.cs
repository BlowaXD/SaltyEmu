using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class BattleEntityStatsManagement
    {
        public static void CheckEntity(IBattleEntity entity)
        {
            if (entity is IPlayerEntity session)
            {
                session.SendPacket(session.GenerateStatPacket());
            }
        }

        public static void DecreaseMp(this IBattleEntity entity, int mp)
        {
            entity.Mp -= mp;
            CheckEntity(entity);
        }

        public static void DecreaseHp(this IBattleEntity entity, int hp)
        {
            entity.Hp -= hp;
            CheckEntity(entity);
        }

        public static void IncreaseMp(this IBattleEntity entity, int mp)
        {
            entity.Mp += mp;
            CheckEntity(entity);
        }

        public static void IncreaseHp(this IBattleEntity entity, int hp)
        {
            entity.Hp += hp;
            CheckEntity(entity);
        }
    }
}