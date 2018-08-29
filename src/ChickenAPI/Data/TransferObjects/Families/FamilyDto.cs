using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Families;

namespace ChickenAPI.Game.Data.TransferObjects.Families
{
    public class FamilyDto : IMappedDto
    {
        public long Id { get; set; }
        public int FamilyExperience { get; set; }

        public GenderType FamilyHeadGender { get; set; }

        public long FamilyId { get; set; }

        public byte FamilyLevel { get; set; }

        public string FamilyMessage { get; set; }

        public byte FamilyFaction { get; set; }

        public FamilyAuthorityType ManagerAuthorityType { get; set; }

        public bool ManagerCanGetHistory { get; set; }

        public bool ManagerCanInvite { get; set; }

        public bool ManagerCanNotice { get; set; }

        public bool ManagerCanShout { get; set; }

        public byte MaxSize { get; set; }

        public FamilyAuthorityType MemberAuthorityType { get; set; }

        public bool MemberCanGetHistory { get; set; }

        public string Name { get; set; }

        public byte WarehouseSize { get; set; }
    }
}