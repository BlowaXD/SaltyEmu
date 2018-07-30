using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;

namespace ChickenAPI.Game.Components
{
    public class ExperienceComponent : IComponent
    {
        public ExperienceComponent(IEntity entity) => Entity = entity;
    
        public byte Level { get; set; }
        public int LevelXp { get; set; }

        public byte HeroLevel { get; set; }
        public int HeroLevelXp { get; set; }

        public byte JobLevel { get; set; }
        public int JobLevelXp { get; set; }

        public IEntity Entity { get; }
    }
}