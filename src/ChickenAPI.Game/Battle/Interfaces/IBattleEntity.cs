using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Movements;

namespace ChickenAPI.Game.Battle.Interfaces
{
    public interface IBattleEntity : IMovableEntity, ISkillEntity
    {
        /// <summary>
        /// Tells if the entity is alive
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Gives the actual Hp Percentage (0-100%)
        /// </summary>
        byte HpPercentage { get; }

        /// <summary>
        /// Gives the actual Mp Percentage (0-100%)
        /// </summary>
        byte MpPercentage { get; }

        int Hp { get; set; }
        int Mp { get; set; }

        int HpMax { get; set; }
        int MpMax { get; set; }
        BattleComponent Battle { get; }
    }
}