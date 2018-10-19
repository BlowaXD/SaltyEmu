using ChickenAPI.Data.Character;
using ChickenAPI.Data.Families;

namespace ChickenAPI.Game.Families
{
    public interface IFamilyCapacities
    {
        /// <summary>
        /// Tells if the player actually has a family
        /// </summary>
        bool HasFamily { get; }

        /// <summary>
        /// Tells if the player is actually it's family's leader
        /// </summary>
        bool IsFamilyLeader { get; }

        /// <summary>
        /// Player's Family
        /// </summary>
        FamilyDto Family { get; set; }

        /// <summary>
        /// Player's Family's related infos
        /// Exp, Join time...
        /// </summary>
        CharacterFamilyDto FamilyCharacter { get; set; }
    }
}