using AutoMapper;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Game.Data.AccessLayer.NpcMonster;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.NpcMonster;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.NpcMonster
{
    public class NpcMonsterSkillDao : MappedRepositoryBase<NpcMonsterSkillDto, NpcMonsterSkillModel>, INpcMonsterSkillService
    {
        public NpcMonsterSkillDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}