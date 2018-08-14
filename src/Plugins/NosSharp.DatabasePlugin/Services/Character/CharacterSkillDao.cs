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
    public class CharacterSkillDao : SynchronizedRepositoryBase<CharacterSkillDto, CharacterSkillModel>, ICharacterSkillService
    {
        public CharacterSkillDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<CharacterSkillDto> GetByCharacterId(long characterId)
        {
            try
            {
                return DbSet.Where(s => s.CharacterId == characterId).ToArray().Select(Mapper.Map<CharacterSkillDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_CHARACTER_ID]", e);
                return null;
            }
        }

        public async Task<IEnumerable<CharacterSkillDto>> GetByCharacterIdAsync(long characterId)
        {
            try
            {
                return (await DbSet.Where(s => s.CharacterId == characterId).ToArrayAsync()).Select(Mapper.Map<CharacterSkillDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_CHARACTER_ID]", e);
                return null;
            }

        }
    }
}