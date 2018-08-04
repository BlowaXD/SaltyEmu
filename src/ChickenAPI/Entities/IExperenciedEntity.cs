using ChickenAPI.Game.Features.Leveling;
using ChickenAPI.Game.Game.Components;

namespace ChickenAPI.Game.Entities
{
    public interface IExperenciedEntity
    {
        ExperienceComponent Experience { get; }
    }
}