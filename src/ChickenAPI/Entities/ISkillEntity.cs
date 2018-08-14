using ChickenAPI.Game.Features.Skills;

namespace ChickenAPI.Game.Entities
{
    public interface ISkillEntity
    {
        SkillComponent Skills { get; }
    }
}