using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Character;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using SaltyEmu.Database.MongoDB;

namespace SaltyEmu.CharacterSkillService.DAO
{
    public class CharacterSkillServiceDao : SynchronizedMongoRepository<CharacterSkillDto>, ICharacterSkillService
    {
        public CharacterSkillServiceDao(MongoConfigurationBuilder builder) : base(builder)
        {
        }

        public IEnumerable<CharacterSkillDto> GetByCharacterId(long characterId)
        {
            return Collection.Find(dto => dto.CharacterId == characterId).ToList();
        }

        public async Task<IEnumerable<CharacterSkillDto>> GetByCharacterIdAsync(long characterId)
        {
            return await (await Collection.FindAsync(dto => dto.CharacterId == characterId)).ToListAsync();
        }
    }
}