using System;
using ChickenAPI.Enums.Game.Families;

namespace ChickenAPI.Data.Character
{
    public class CharacterFamilyDto : IMappedDto
    {
        public long Id { get; set; }

        public long CharacterId { get; set; }


        public FamilyAuthority Authority { get; set; }

        public string DailyMessage { get; set; }

        public int Experience { get; set; }

        public long FamilyId { get; set; }

        public FamilyMemberRank Rank { get; set; }

        public DateTime JoinDate { get; set; }
    }
}