using System.Linq;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Buffs;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class BattleBuffExtensions
    {
        public static void AddBuff(this IBattleEntity entity, BuffContainer buffContainer)
        {
            if (entity.GetBuffByCardId(buffContainer.Id) == null)
            {
                entity.Buffs.Add(buffContainer);
            }
        }

        public static void RemoveBuffById(this IBattleEntity entity, long id)
        {
            BuffContainer buffContainer = entity.GetBuffByCardId(id);

            if (buffContainer != null)
            {
                entity.Buffs.Remove(buffContainer);
            }
        }

        public static bool HasBuff(this IBattleEntity entity, long id) => GetBuffByCardId(entity, id) != null;

        public static BuffContainer GetBuffByCardId(this IBattleEntity entity, long id)
        {
            return entity.Buffs.FirstOrDefault(s => s.Id == id);
        }
    }
}