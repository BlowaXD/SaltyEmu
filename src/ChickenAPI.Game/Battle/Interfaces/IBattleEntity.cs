using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Movements;

namespace ChickenAPI.Game.Battle.Interfaces
{
    public interface IBattleEntity : IMovableEntity, ISkillEntity
    {
        long Hp { get; }
        long Mp { get; }

        long HpMax { get; }
        long MpMax { get; }
        BattleComponent Battle { get; }
    }
}