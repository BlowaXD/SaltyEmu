using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Entities
{
    public interface IExperenciedEntity : IEntity
    {
        byte Level { get; set; }
        long LevelXp { get; set; }

        byte HeroLevel { get; set; }
        long HeroLevelXp { get; set; }

        byte JobLevel { get; set; }
        long JobLevelXp { get; set; }
    }
}