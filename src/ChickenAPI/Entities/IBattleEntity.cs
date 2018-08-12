using ChickenAPI.Game.Features.Battle;

namespace ChickenAPI.Game.Entities
{
    public interface IBattleEntity
    {
        BattleComponent Battle { get; }
    }
}