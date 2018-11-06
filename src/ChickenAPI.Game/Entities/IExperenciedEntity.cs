namespace ChickenAPI.Game.Entities
{
    public interface IExperenciedEntity
    {
        byte Level { get; set; }
        long LevelXp { get; set; }

        byte HeroLevel { get; set; }
        long HeroLevelXp { get; set; }

        byte JobLevel { get; set; }
        long JobLevelXp { get; set; }
    }
}