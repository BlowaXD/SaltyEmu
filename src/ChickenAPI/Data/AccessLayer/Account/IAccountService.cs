using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Character;

namespace ChickenAPI.Game.Data.AccessLayer.Account
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