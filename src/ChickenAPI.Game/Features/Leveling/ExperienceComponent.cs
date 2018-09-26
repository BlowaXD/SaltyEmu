using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Game.Features.Leveling
{
    public class ExperienceComponent : IComponent
    {
        public ExperienceComponent(IEntity entity) => Entity = entity;

        public byte Level { get; set; }
        public long LevelXp { get; set; }

        public byte HeroLevel { get; set; }
        public long HeroLevelXp { get; set; }

        public byte JobLevel { get; set; }
        public long JobLevelXp { get; set; }

        public IEntity Entity { get; }
    }
}