using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Enums.Game.Families;

namespace ChickenAPI.Game.Data.TransferObjects.Character
{
    public class CharacterFamilyDto : IMappedDto
    {
        public long Id { get; set; }


        public FamilyAuthority Authority { get; set; }

        public long CharacterId { get; set; }

        public string DailyMessage { get; set; }

        public int Experience { get; set; }

        public long FamilyCharacterId { get; set; }

        public long FamilyId { get; set; }

        public FamilyMemberRank Rank { get; set; }
    }
}