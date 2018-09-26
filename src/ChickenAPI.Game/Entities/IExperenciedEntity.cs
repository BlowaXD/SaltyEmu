using ChickenAPI.Game.Features.Leveling;

namespace ChickenAPI.Game.Entities
{
    public interface IExperenciedEntity
    {
        ExperienceComponent Experience { get; }
    }
}