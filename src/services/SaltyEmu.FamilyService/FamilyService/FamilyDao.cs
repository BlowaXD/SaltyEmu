using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Families;
using ChickenAPI.Game.Data.AccessLayer.Families;
using Foundatio.Utility;
using SaltyEmu.Redis;
using SaltyEmu.RedisWrappers;

namespace SaltyEmu.FamilyService.FamilyService
{
    public class FamilyDao : MappedCacheClientBase<FamilyDto>, IFamilyService
    {
        protected FamilyDao(RedisConfiguration conf) : base(conf)
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