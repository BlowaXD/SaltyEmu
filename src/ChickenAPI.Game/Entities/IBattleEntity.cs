using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Features.Battle;

namespace ChickenAPI.Game.Entities
{
    public interface IBattleEntity : IEntity
    {
        BattleComponent Battle { get; }
    }
}