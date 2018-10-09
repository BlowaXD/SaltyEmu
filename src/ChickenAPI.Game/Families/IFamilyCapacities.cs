using ChickenAPI.Data.Character;
using ChickenAPI.Data.Families;

namespace ChickenAPI.Game.Families
{
    public interface IFamilyCapacities
    {
        /// <summary>
        /// Joins the given family
        /// Will emit the <seealso cref="Events.FamilyJoinEvent"/>
        /// </summary>
        void FamilyJoin(FamilyDto dto);

        /// <summary>
        /// Leaves the family
        /// Does nothing if the character has no family
        /// </summary>
        void FamilyLeave();


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
        CharacterFamilyDto FamilyCharacter { get; set; }
    }
}