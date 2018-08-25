using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.TransferObjects.Character;
using Microsoft.EntityFrameworkCore;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models.Character;
using NosSharp.DatabasePlugin.Services.Base;

namespace NosSharp.DatabasePlugin.Services.Character
{
    public class CharacterQuickListDao : SynchronizedRepositoryBase<CharacterQuicklistDto, CharacterQuicklistModel>, ICharacterQuickListService
    {
        #region Methods

        private readonly CharacterQuicklistDto _baseConf;

        public CharacterQuickListDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
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