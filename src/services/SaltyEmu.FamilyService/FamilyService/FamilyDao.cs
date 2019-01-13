using System;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data.Families;
using SaltyEmu.Redis;

namespace SaltyEmu.FamilyService.FamilyService
{
    public class FamilyDao : MappedCacheClientBase<FamilyDto>, IFamilyService
    {
        public FamilyDao(RedisConfiguration conf) : base(conf)
        {
        }

        public FamilyDto GetByName(string name)
        {
            return Get().FirstOrDefault(s => s.Name == name);
        }

        public async Task<FamilyDto> GetByNameAsync(string name)
        {
            return (await GetAsync()).FirstOrDefault(s => s.Name == name);
        }

        public void UpdateFamily(long familyId)
        {
            throw new NotImplementedException();
        }
    }
}