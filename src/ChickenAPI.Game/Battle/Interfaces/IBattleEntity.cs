using System;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Movements;

namespace ChickenAPI.Game.Battle.Interfaces
{
    public interface IBattleEntity : IBattleCapacity, IMovableEntity, ISkillEntity, IExperenciedEntity
    {
    }
}