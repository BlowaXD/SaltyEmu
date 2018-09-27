using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.Movements;

namespace ChickenAPI.Game.Battle.Interfaces
{
    public interface IBattleEntity : IMovableEntity
    {
        BattleComponent Battle { get; }
    }
}