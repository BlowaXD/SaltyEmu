using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.NpcMonster;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.NpcMonster;

namespace SaltyEmu.DatabasePlugin.Services.NpcMonster
{
    public class NpcMonsterSkillDao : MappedRepositoryBase<NpcMonsterSkillDto, NpcMonsterSkillModel>, INpcMonsterSkillService
    {
        private readonly Dictionary<long, NpcMonsterSkillDto[]> _cacheByNpcMonsterId;

        public NpcMonsterSkillDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
            IEnumerable<NpcMonsterSkillDto> tmp = Get();
            _cacheByNpcMonsterId = tmp.GroupBy(s => s.NpcMonsterId).ToDictionary(s => s.Key, s => s.ToArray());
        }


        public async Task<IEnumerable<NpcMonsterSkillDto>> GetByNpcMonsterIdsAsync(IEnumerable<long> ids)
        {
            // faster temporary implementation
            List<NpcMonsterSkillDto> list = new List<NpcMonsterSkillDto>();
            foreach (long id in ids)
            {
                if (!_cacheByNpcMonsterId.TryGetValue(id, out NpcMonsterSkillDto[] dto))
                {
                    dto = (await DbSet.Where(s => ids.Any(idp => idp == s.NpcMonsterId)).ToArrayAsync()).Select(Mapper.Map<NpcMonsterSkillDto>).ToArray();
                    _cacheByNpcMonsterId[id] = dto;
                }

                list.AddRange(dto);
            }

            return list;
        }
    }
}