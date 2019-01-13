using ChickenAPI.Data.Account;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Character;

namespace ChickenAPI.Game.Builders
{
    public class CharacterBuilder
    {
        private long _accountId;
        private CharacterClassType _class;
        private FactionType _faction;
        private GenderType _gender;
        private long _gold;
        private byte _jobLevel;
        private byte _level;
        private string _name;
        private byte _slot;

        public CharacterBuilder WithFaction(FactionType faction)
        {
            _faction = faction;
            return this;
        }

        public CharacterBuilder ForAccount(AccountDto accountDto)
        {
            _accountId = accountDto.Id;
            return this;
        }

        public CharacterBuilder WithGender(GenderType gender)
        {
            _gender = gender;
            return this;
        }

        public CharacterBuilder WithClass(CharacterClassType classType)

        {
            _class = classType;
            return this;
        }

        public CharacterBuilder InSlot(byte slot)
        {
            _slot = slot;
            return this;
        }

        public CharacterBuilder WithGold(long gold)
        {
            _gold = gold;
            return this;
        }

        public CharacterBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CharacterBuilder HasLevel(byte level)
        {
            _level = level;
            return this;
        }

        public CharacterBuilder HasJobLevel(byte jobLevel)
        {
            _jobLevel = jobLevel;
            return this;
        }


        public CharacterDto Build() =>
            new CharacterDto
            {
                Name = _name,
                AccountId = _accountId,
                Class = CharacterClassType.Adventurer,
                Slot = _slot,
                Faction = _faction,
                Gender = _gender,
                Gold = _gold,
                Level = _level,
                JobLevel = _jobLevel
            };

        public static implicit operator CharacterDto(CharacterBuilder builder) => builder.Build();
    }
}