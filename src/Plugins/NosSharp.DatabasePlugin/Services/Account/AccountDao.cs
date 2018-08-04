using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.TransferObjects;
using ChickenAPI.Game.Data.AccessLayer.Account;
using ChickenAPI.Game.Data.TransferObjects.Character;
using Microsoft.EntityFrameworkCore;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models.Character;
using NosSharp.DatabasePlugin.Services.Base;
using NosSharp.DatabasePlugin.Utils;

namespace NosSharp.DatabasePlugin.Services.Account
{
    public class AccountDao : MappedRepositoryBase<AccountDto, AccountModel>, IAccountService
    {
        public AccountDao(NosSharpContext dbFactory, IMapper mapper) : base(dbFactory, mapper)
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