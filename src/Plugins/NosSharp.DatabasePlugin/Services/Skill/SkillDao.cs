using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Skills;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.Skill;

namespace SaltyEmu.DatabasePlugin.Services.Skill
{
    public class SkillDao : MappedRepositoryBase<SkillDto, SkillModel>, ISkillService
    {
        private readonly Dictionary<long, SkillDto> _skills;
        private readonly Dictionary<long, SkillDto[]> _skillsByClassId;

        public SkillDao(DbContext context, IMapper mapper, ILogger log) : base(context, mapper, log)
        {
            IEnumerable<SkillDto> tmp = Get();
            _skills = tmp.ToDictionary(s => s.Id, s => s);
            _skillsByClassId = tmp.GroupBy(s => (long)s.Class).ToDictionary(s => s.Key, s => s.ToArray());
        }

        public override SkillDto GetById(long id)
        {
            if (_skills.TryGetValue(id, out SkillDto value))
            {
                return value;
            }

            value = base.GetById(id);
            _skills[id] = value;

            return value;
        }


        public SkillDto[] GetByClassId(byte classId)
        {
            if (!_skillsByClassId.TryGetValue(classId, out SkillDto[] skills))
            {
                skills = DbSet.Where(s => s.Class == classId).ToArray().Select(Mapper.Map<SkillDto>).ToArray();
                _skillsByClassId[classId] = skills;
            }

            return skills;
        }

        public async Task<SkillDto[]> GetByClassIdAsync(byte classId)
        {
            if (!_skillsByClassId.TryGetValue(classId, out SkillDto[] skills))
            {
                skills = (await DbSet.Where(s => s.Class == classId).ToArrayAsync()).Select(Mapper.Map<SkillDto>).ToArray();
                _skillsByClassId[classId] = skills;
            }

            return skills;
        }

        public override async Task<SkillDto> GetByIdAsync(long id)
        {
            if (_skills.TryGetValue(id, out SkillDto value))
            {
                return value;
            }

            value = await base.GetByIdAsync(id);
            _skills[id] = value;

            return value;
        }
    }
}