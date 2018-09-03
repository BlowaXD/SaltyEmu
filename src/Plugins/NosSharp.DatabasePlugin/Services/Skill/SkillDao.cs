using AutoMapper;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Skill;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Skill
{
    public class SkillDao : MappedRepositoryBase<SkillDto, SkillModel>, ISkillService
    {
        public SkillDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}