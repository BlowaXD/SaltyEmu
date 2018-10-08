using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Families;
using ChickenAPI.Game.Data.AccessLayer.Families;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Families;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Families
{
    public class FamilyDao : MappedRepositoryBase<FamilyDto, FamilyModel>, IFamilyService
    {
        public FamilyDao(SaltyDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public FamilyDto GetByName(string name)
        {
            return Mapper.Map<FamilyDto>(DbSet.SingleOrDefault(s => s.Name == name));
        }

        public async Task<FamilyDto> GetByNameAsync(string name)
        {
            return Mapper.Map<FamilyDto>(await DbSet.SingleOrDefaultAsync(s => s.Name == name));
        }

        public void UpdateFamily(long familyId)
        {
            throw new System.NotImplementedException();
        }
    }
}