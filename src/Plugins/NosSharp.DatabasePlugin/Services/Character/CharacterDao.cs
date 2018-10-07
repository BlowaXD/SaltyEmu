using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.AccessLayer.Character;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Character;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Character
{
    public class CharacterDao : MappedRepositoryBase<CharacterDto, CharacterModel>, ICharacterService
    {
        private readonly CharacterDto _baseConf;

        public CharacterDao(SaltyDbContext context, IMapper mapper, CharacterDto baseConf) : base(context, mapper) => _baseConf = baseConf;

        public IEnumerable<CharacterDto> GetActiveByAccountId(long accountId)
        {
            try
            {
                return DbSet.Where(s => s.AccountId == accountId && s.State == CharacterState.Active).ToList().Select(Mapper.Map<CharacterDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_ACTIVE_BY_ACCOUNT_ID]", e);
                return null;
            }
        }

        public async Task<IEnumerable<CharacterDto>> GetActiveByAccountIdAsync(long accountId)
        {
            try
            {
                return (await DbSet.Where(s => s.AccountId == accountId && s.State == CharacterState.Active).ToListAsync()).Select(Mapper.Map<CharacterDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_ACTIVE_BY_ACCOUNT_ID]", e);
                return null;
            }
        }

        public CharacterDto GetByAccountIdAndSlot(long accountId, byte slot)
        {
            try
            {
                return Mapper.Map<CharacterDto>(DbSet.SingleOrDefault(s => s.AccountId == accountId && s.Slot == slot && s.State == CharacterState.Active));
            }
            catch (Exception e)
            {
                Log.Error("[GET_ACTIVE_BY_ACCOUNT_ID]", e);
                return null;
            }
        }

        public async Task<CharacterDto> GetByAccountIdAndSlotAsync(long accountId, byte slot)
        {
            try
            {
                return Mapper.Map<CharacterDto>(await DbSet.SingleOrDefaultAsync(s => s.AccountId == accountId && s.Slot == slot && s.State == CharacterState.Active));
            }
            catch (Exception e)
            {
                Log.Error("[GET_ACTIVE_BY_ACCOUNT_ID]", e);
                return null;
            }
        }

        public async Task<CharacterDto> GetActiveByNameAsync(string characterName)
        {
            try
            {
                return Mapper.Map<CharacterDto>(await DbSet.SingleOrDefaultAsync(s => s.Name == characterName && s.State == CharacterState.Active));
            }
            catch (Exception e)
            {
                Log.Error("[GET_ACTIVE_BY_ACCOUNT_ID]", e);
                return null;
            }
        }

        public override void DeleteById(long id)
        {
            try
            {
                CharacterModel model = DbSet.Find(id);
                if (model == null)
                {
                    return;
                }

                model.State = CharacterState.Inactive;
                DbSet.Update(model);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error("[DELETE_BY_ID]", e);
            }
        }

        public override async Task DeleteByIdAsync(long id)
        {
            try
            {
                CharacterModel model = await DbSet.FindAsync(id);
                if (model == null)
                {
                    return;
                }

                model.State = CharacterState.Inactive;
                DbSet.Update(model);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error("[DELETE_BY_ID]", e);
            }
        }

        public CharacterDto GetCreationCharacter() => Mapper.Map<CharacterDto>(Mapper.Map<CharacterModel>(_baseConf));
    }
}