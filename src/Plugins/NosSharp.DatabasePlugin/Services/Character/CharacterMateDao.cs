using System;
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
    public class CharacterMateDao : MappedRepositoryBase<CharacterMateDto, CharacterMateModel>, ICharacterMateService
    {
        public CharacterMateDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public CharacterMateDto[] GetMatesByCharacterId(long characterId)
        {
            try
            {
                return DbSet.Where(s => s.CharacterId == characterId).ToArray().Select(Mapper.Map<CharacterMateDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_IDS]", e);
                return null;
            }
        }

        public async Task<CharacterMateDto[]> GetMatesByCharacterIdAsync(long characterId)
        {
            try
            {
                return (await DbSet.Where(s => s.CharacterId == characterId).ToArrayAsync()).Select(Mapper.Map<CharacterMateDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_IDS]", e);
                return null;
            }
        }
    }
}