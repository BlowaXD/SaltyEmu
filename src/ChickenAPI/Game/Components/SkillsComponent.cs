using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Game.Components
{
    public class SkillsComponent : IComponent
    {
        public SkillsComponent(IEntity entity) => Entity = entity;

        public Dictionary<long, SkillDto> Skills { get; }

        public IEntity Entity { get; }
    }
}