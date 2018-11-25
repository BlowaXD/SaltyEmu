using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Game.Data.AccessLayer.NpcMonster;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.NpcMonster;

namespace SaltyEmu.DatabasePlugin.Services.NpcMonster
{
    public class NpcMonsterSkillDao : MappedRepositoryBase<NpcMonsterSkillDto, NpcMonsterSkillModel>, INpcMonsterSkillService
    {
        public NpcMonsterSkillDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }


    }
}