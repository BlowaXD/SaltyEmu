using ChickenAPI.Data;
using ChickenAPI.Data.Families;

namespace ChickenAPI.Game.Data.AccessLayer.Families
{
    public interface IFamilyService : IMappedRepository<FamilyDto>
    {
        FamilyDto GetByName(string creationFamilyName);

        /// <summary>
        /// Broadcast an UpdateFamily event that will force sync on clients
        /// Will use ISC
        /// </summary>
        /// <param name="familyId"></param>
        void UpdateFamily(long familyId);
    }
}