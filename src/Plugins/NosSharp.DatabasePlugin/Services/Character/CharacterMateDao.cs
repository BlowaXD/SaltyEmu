using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Data.AccessLayer.Character;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Character;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Character
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