using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Character;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Character;

namespace SaltyEmu.DatabasePlugin.Services.Character
{
    public class CharacterMateDao : MappedRepositoryBase<CharacterMateDto, CharacterMateModel>, ICharacterMateService
    {
        public CharacterMateDao(SaltyDbContext context, IMapper mapper, ILogger log) : base(context, mapper, log)
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