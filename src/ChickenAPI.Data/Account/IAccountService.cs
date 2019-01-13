using System.Threading.Tasks;

namespace ChickenAPI.Data.Account
{
    public interface IAccountService : IMappedRepository<AccountDto>
    {
        /// <summary>
        ///     Will return the AccountDto associated to name given as parameter
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        AccountDto GetByName(string name);

        /// <summary>
        ///     Will asynchronously return the AccountDto associated to name given as parameter
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<AccountDto> GetByNameAsync(string name);
    }
}