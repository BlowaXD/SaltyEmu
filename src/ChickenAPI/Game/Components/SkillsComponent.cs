using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Game.Components
{
    public class SkillsComponent : IComponent
    {
        public SkillsComponent(IEntity entity)
        {
            Entity = entity;
        }

        public IEntity Entity { get; }

        public Dictionary<long, SkillDto> Skills { get; }
    }
}