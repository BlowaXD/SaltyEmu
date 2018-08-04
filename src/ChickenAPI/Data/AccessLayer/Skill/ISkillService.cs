using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Data.AccessLayer.Skill
{
    public interface ISkillService : IMappedRepository<SkillDto>
    {
    }
}