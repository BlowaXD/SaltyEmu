using AutoMapper;
using ChickenAPI.Game.Data.AccessLayer.NpcMonster;
using ChickenAPI.Game.Data.TransferObjects.NpcMonster;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models.NpcMonster;
using NosSharp.DatabasePlugin.Services.Base;

namespace NosSharp.DatabasePlugin.Services.NpcMonster
{
    public class NpcMonsterSkillDao : MappedRepositoryBase<NpcMonsterSkillDto, NpcMonsterSkillModel>, INpcMonsterSkillService
    {
        public NpcMonsterSkillDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}