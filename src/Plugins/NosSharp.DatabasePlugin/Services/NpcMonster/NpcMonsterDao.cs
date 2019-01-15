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
    public class NpcMonsterDao : MappedRepositoryBase<NpcMonsterDto, NpcMonsterModel>, INpcMonsterService
    {
        private readonly Dictionary<long, NpcMonsterDto> _monsters;

        public NpcMonsterDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
            _monsters = Get().ToDictionary(s => s.Id, s => s);
        }

        public override NpcMonsterDto GetById(long id)
        {
            if (_monsters.TryGetValue(id, out NpcMonsterDto value))
            {
                return value;
            }

            value = base.GetById(id);
            _monsters[id] = value;

            return value;
        }

        public override async Task<NpcMonsterDto> GetByIdAsync(long id)
        {
            if (_monsters.TryGetValue(id, out NpcMonsterDto value))
            {
                return value;
            }

            value = await base.GetByIdAsync(id);
            _monsters[id] = value;

            return value;
        }
    }
}