using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Drop;
using ChickenAPI.Game.Data.AccessLayer.Drop;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Drop
{
    // todo finish that I'm too lazy for all subtypes
    /// <summary>
    /// 
    /// </summary>
    public class DropDao : MappedRepositoryBase<DropDto, DropModel>, IDropService
    {
        private readonly Dictionary<long, DropDto> _items = new Dictionary<long, DropDto>();

        public DropDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
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

        public Task<IEnumerable<DropDto>> GetByMapNpcMonsterIdAsync(long mapNpcMonsterId)
        {
            try
            {
                return null;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_NPCMONSTER_ID]", e);
                return null;
            }
        }
    }
}