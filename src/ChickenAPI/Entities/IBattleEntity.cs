using ChickenAPI.Game.Features.Battle;
using ChickenAPI.Game.Game.Components;

namespace ChickenAPI.Game.Entities
{
    public interface IBattleEntity
    {
        BattleComponent Battle { get; }
    }
}