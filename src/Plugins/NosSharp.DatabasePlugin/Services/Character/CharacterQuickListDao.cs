using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.TransferObjects.Character;
using Microsoft.EntityFrameworkCore;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models.Character;
using NosSharp.DatabasePlugin.Services.Base;

namespace NosSharp.DatabasePlugin.Services.Character
{
    public class CharacterQuickListDao : SynchronizedRepositoryBase<CharacterQuickListDto, CharacterQuikListModel>, ICharacterQuickListService
    {
        #region Methods
        private readonly CharacterQuickListDto _baseConf;

        public CharacterQuickListDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<CharacterQuickListDto> LoadByCharacterId(long id)
        {
            try
            {
                return (DbSet.Where(s => s.CharacterId == id).ToArray()).Select(Mapper.Map<CharacterQuickListDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_CHARACTER_ID]", e);
                return null;
            }
        }

        public virtual async Task<IEnumerable<CharacterQuickListDto>> LoadKeysByCharacterIdAsync(IEnumerable<Guid> ids)
        {
            try
            {
                return (await DbSet.Where(s => ids.Contains(s.Id)).ToListAsync()).Select(Mapper.Map<CharacterQuickListDto>);
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