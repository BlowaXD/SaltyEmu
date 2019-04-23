using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Packets.Enumerations;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.Character;

namespace SaltyEmu.DatabasePlugin.Models.Families
{
    public class FamilyModel : IMappedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public uint Experience { get; set; }

        public GenderType FamilyHeadGender { get; set; }

        public byte Level { get; set; }

        [MaxLength(255)]
        public string FamilyMessage { get; set; }

        public FactionType FamilyFaction { get; set; }

        public FamilyAuthorityType ManagerAuthorityType { get; set; }

        public bool ManagerCanGetHistory { get; set; }

        public bool ManagerCanInvite { get; set; }

        public bool ManagerCanNotice { get; set; }

        public bool ManagerCanShout { get; set; }

        public short MaxSize { get; set; }

        public FamilyAuthorityType MemberAuthorityType { get; set; }

        public bool MemberCanGetHistory { get; set; }

        [MaxLength(32)]
        public string Name { get; set; }

        public byte WarehouseSize { get; set; }
        public virtual IEnumerable<CharacterFamilyModel> FamilyMembers { get; set; }
    }
}