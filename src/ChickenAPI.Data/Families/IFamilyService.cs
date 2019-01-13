using System.Threading.Tasks;

namespace ChickenAPI.Data.Families
{
    public interface IFamilyService : IMappedRepository<FamilyDto>
    {
        /// <summary>
        ///     Gets the family by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        FamilyDto GetByName(string name);

        /// <summary>
        ///     Asynchronously gets the family object by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<FamilyDto> GetByNameAsync(string name);

        /// <summary>
        ///     Broadcast an UpdateFamily event that will force sync on clients
        ///     Will use ISC
        /// </summary>
        /// <param name="familyId"></param>
        void UpdateFamily(long familyId);
    }
}