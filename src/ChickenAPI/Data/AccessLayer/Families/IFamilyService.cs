using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Families;

namespace ChickenAPI.Game.Data.AccessLayer.Families
{
    public interface IFamilyService : IMappedRepository<FamilyDto>
    {
        /// <summary>
        /// Broadcast an UpdateFamily event that will force sync on clients
        /// Will use ISC
        /// </summary>
        /// <param name="familyId"></param>
        void UpdateFamily(long familyId);
    }
}