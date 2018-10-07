using ChickenAPI.Data.Character;
using ChickenAPI.Data.Families;

namespace ChickenAPI.Game.Families
{
    public interface IFamilyEntity
    {
        bool HasFamily { get; }
        bool IsFamilyLeader { get; }
        FamilyDto Family { get; set; }
        CharacterFamilyDto FamilyCharacter { get; set; }
    }
}