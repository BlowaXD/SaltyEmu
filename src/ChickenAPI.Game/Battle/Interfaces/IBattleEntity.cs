using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Movements;
using ChickenAPI.Game.Skills;

namespace ChickenAPI.Game.Battle.Interfaces
{
    public interface IBattleEntity : IBattleCapacity, IMovableEntity, ISkillEntity, IExperenciedEntity
    {
    }
}