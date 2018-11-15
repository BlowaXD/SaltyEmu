using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Skills;

namespace ChickenAPI.Game.Data.AccessLayer.Skill
{
    public interface ISkillService : IMappedRepository<SkillDto>
    {
        SkillDto[] GetByClassId(byte classId);

        Task<SkillDto[]> GetByClassIdAsync(byte classId);
    }
}