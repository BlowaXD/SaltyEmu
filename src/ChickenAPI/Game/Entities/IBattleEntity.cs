using ChickenAPI.Game.Components;

namespace ChickenAPI.Game.Entities
{
    public interface IBattleEntity
    {
        BattleComponent Battle { get; }
    }
}