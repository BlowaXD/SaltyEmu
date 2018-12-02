using System.Threading.Tasks;

namespace ChickenAPI.Data.Skills
{
    public interface ISkillService : IMappedRepository<SkillDto>
    {
        SkillDto[] GetByClassId(byte classId);

        Task<SkillDto[]> GetByClassIdAsync(byte classId);
    }
}