using System.Collections.Generic;
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
    public class CharacterFamilyDao : MappedRepositoryBase<CharacterFamilyDto, CharacterFamilyModel>, ICharacterFamilyService
    {
        public CharacterFamilyDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public CharacterFamilyDto GetCharacterFamilyByCharacterId(long characterId)
        {
            return Mapper.Map<CharacterFamilyDto>(DbSet.SingleOrDefault(s => s.CharacterId == characterId));
        }

        public async Task<CharacterFamilyDto> GetCharacterFamilyByCharacterIdAsync(long characterId)
        {
            return Mapper.Map<CharacterFamilyDto>(await DbSet.SingleOrDefaultAsync(s => s.CharacterId == characterId));
        }

        public IEnumerable<CharacterFamilyDto> GetCharacterFamilyDtosByFamilyId(long familyId)
        {
            return DbSet.Where(s => s.FamilyId == familyId).ToArray().Select(Mapper.Map<CharacterFamilyDto>);
        }

        public async Task<IEnumerable<CharacterFamilyDto>> GetCharacterFamilyDtosByFamilyIdAsync(long familyId)
        {
            return (await DbSet.Where(s => s.FamilyId == familyId).ToArrayAsync()).Select(Mapper.Map<CharacterFamilyDto>);
        }
    }
}