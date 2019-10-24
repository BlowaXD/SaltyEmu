using System.ComponentModel.DataAnnotations;
using ChickenAPI.Data.Enums.Game.Families;
using ChickenAPI.Packets.Enumerations;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.Families;

namespace SaltyEmu.DatabasePlugin.Models.Character
{
    public class CharacterFamilyModel : IMappedModel
    {
        public long Id { get; set; }
        public FamilyAuthority Authority { get; set; }

        public virtual CharacterModel Character { get; set; }

        public long CharacterId { get; set; }

        [MaxLength(255)]
        public string DailyMessage { get; set; }

        public int Experience { get; set; }

        public virtual FamilyModel Family { get; set; }

        public long FamilyCharacterId { get; set; }

        public long FamilyId { get; set; }

        public FamilyMemberRank Rank { get; set; }
    }
}