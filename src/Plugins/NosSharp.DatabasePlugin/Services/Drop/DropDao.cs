using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Drop;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.Drops;

namespace SaltyEmu.DatabasePlugin.Services.Drop
{
    // todo finish that I'm too lazy for all subtypes
    /// <summary>
    /// 
    /// </summary>
    public class DropDao : MappedRepositoryBase<DropDto, DropModel>, IDropService
    {
        private readonly Dictionary<long, DropDto> _items = new Dictionary<long, DropDto>();

        public DropDao(DbContext context, IMapper mapper, ILogger log) : base(context, mapper, log)
        {
        }

        public override DropDto GetById(long id)
        {
            if (_items.TryGetValue(id, out DropDto value))
            {
                return value;
            }

            value = base.GetById(id);
            _items[id] = value;

            return value;
        }

        public override async Task<DropDto> GetByIdAsync(long id)
        {
            if (_items.TryGetValue(id, out DropDto value))
            {
                return value;
            }

            value = await base.GetByIdAsync(id);
            _items[id] = value;

            return value;
        }

        public async Task<IEnumerable<DropDto>> GetByMapNpcMonsterIdAsync(long mapNpcMonsterId)
        {
            try
            {
                return (await Context.Set<NpcMonsterDropModel>().Where(s => s.TypedId == mapNpcMonsterId).ToListAsync()).Select(Mapper.Map<DropDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_NPCMONSTER_ID]", e);
                return null;
            }
        }
    }
}