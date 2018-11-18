using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Skill;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Skill
{
    public class SkillDao : MappedRepositoryBase<SkillDto, SkillModel>, ISkillService
    {
        private readonly Dictionary<long, SkillDto> _skills;
        private readonly Dictionary<long, SkillDto[]> _skillsByClassId;

        public SkillDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
            IEnumerable<SkillDto> tmp = Get();
            _skills = tmp.ToDictionary(s => s.Id, s => s);
            _skillsByClassId = tmp.GroupBy(s => s.Id).ToDictionary(s => s.Key, s => s.ToArray());
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