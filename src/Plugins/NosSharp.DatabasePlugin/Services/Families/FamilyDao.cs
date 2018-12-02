using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Families;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.Families;

namespace SaltyEmu.DatabasePlugin.Services.Families
{
    public class FamilyDao : MappedRepositoryBase<FamilyDto, FamilyModel>, IFamilyService
    {
        public FamilyDao(DbContext context, IMapper mapper) : base(context, mapper)
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
            // do be done
        }
    }
}