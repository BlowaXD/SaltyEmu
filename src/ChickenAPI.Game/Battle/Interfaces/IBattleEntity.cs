using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Movements;

namespace ChickenAPI.Game.Battle.Interfaces
{
    public interface IBattleEntity : IMovableEntity, ISkillEntity
    {
        int Hp { get; set; }
        int Mp { get; set; }

        int HpMax { get; set; }
        int MpMax { get; set; }
        BattleComponent Battle { get; }
    }
}