using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.AccessLayer.NpcMonster;
using ChickenAPI.Data.TransferObjects.NpcMonster;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models.NpcMonster;
using NosSharp.DatabasePlugin.Services.Base;

namespace NosSharp.DatabasePlugin.Services.NpcMonster
{
    public class NpcMonsterDao : MappedRepositoryBase<NpcMonsterDto, NpcMonsterModel>, INpcMonsterService
    {
        private readonly Dictionary<long, NpcMonsterDto> _monsters = new Dictionary<long, NpcMonsterDto>();

        public NpcMonsterDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
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