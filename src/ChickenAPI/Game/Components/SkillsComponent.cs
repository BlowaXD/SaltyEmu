using System.Collections.Generic;
using ChickenAPI.Data.TransferObjects.Skills;
using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;

namespace ChickenAPI.Game.Components
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