using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Account;
using ChickenAPI.Data.Character;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Character;

namespace SaltyEmu.DatabasePlugin.Services.Account
{
    public class AccountDao : MappedRepositoryBase<AccountDto, AccountModel>, IAccountService
    {
        public AccountDao(DbContext dbFactory, IMapper mapper) : base(dbFactory, mapper)
        {
        }

        public AccountDto GetByName(string name)
        {
            try
            {
                AccountModel account = DbSet.SingleOrDefault(s => s.Name == name);
                return Mapper.Map<AccountDto>(account);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_NAME]", e);
                return null;
            }
        }

        public async Task<AccountDto> GetByNameAsync(string name)
        {
            try
            {
                AccountModel account = await DbSet.SingleOrDefaultAsync(s => s.Name == name);
                return Mapper.Map<AccountDto>(account);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_NAME]", e);
                return null;
            }
        }
    }
}