using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Character;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Character;

namespace SaltyEmu.DatabasePlugin.Services.Character
{
    public class CharacterQuickListDao : SynchronizedRepositoryBase<CharacterQuicklistDto, CharacterQuicklistModel>, ICharacterQuickListService
    {
        #region Methods

        public CharacterQuickListDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<CharacterQuicklistDto> GetByCharacterId(long id)
        {
            try
            {
                return DbSet.Where(s => s.CharacterId == id).ToArray().Select(Mapper.Map<CharacterQuicklistDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_CHARACTER_ID]", e);
                return null;
            }
        }

        public async Task<IEnumerable<CharacterQuicklistDto>> GetByCharacterIdAsync(long characterId)
        {
            try
            {
                return (await DbSet.Where(s => s.CharacterId == characterId).ToListAsync()).Select(Mapper.Map<CharacterQuicklistDto>);
            }
            catch (Exception e)
            {
                Log.Error("[LOAD_KEY_BY_CHARACTER_ID]", e);
                return null;
            }
        }

        #endregion
    }
}