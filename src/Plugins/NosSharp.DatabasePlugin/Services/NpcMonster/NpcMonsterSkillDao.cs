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
        private readonly Dictionary<long, List<NpcMonsterSkillDto>> _cacheByNpcMonsterId;

        public NpcMonsterSkillDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
            IEnumerable<NpcMonsterSkillDto> tmp = Get();
            _cacheByNpcMonsterId = tmp.GroupBy(s => s.NpcMonsterId).ToDictionary(s => s.Key, s => s.ToList());
        }


        public async Task<IEnumerable<NpcMonsterSkillDto>> GetByNpcMonsterIdsAsync(IEnumerable<long> ids)
        {
            // faster temporary implementation
            var list = new List<NpcMonsterSkillDto>();
            foreach (long id in ids)
            {
                if (!_cacheByNpcMonsterId.TryGetValue(id, out List<NpcMonsterSkillDto> dto))
                {
                    continue;
                }

                list.AddRange(dto);
            }

            return list;

            try
            {
                return (await DbSet.Where(s => ids.Any(id => id == s.NpcMonsterId)).ToArrayAsync()).Select(Mapper.Map<NpcMonsterSkillDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_NPC_MONSTER_IDS_ASYNC]", e);
                return new List<NpcMonsterSkillDto>();
            }
        }
    }
}