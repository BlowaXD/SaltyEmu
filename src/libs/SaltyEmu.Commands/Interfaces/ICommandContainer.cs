using System.Threading.Tasks;
using Qmmands;
using SaltyEmu.Commands.Entities;

namespace SaltyEmu.Commands.Interfaces
{
    public interface ICommandContainer
    {
        /// <summary>
        /// Asynchronously add a module to CommandContainer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task AddModuleAsync<T>() where T : SaltyModuleBase;

        Task InitializeAsync();
        Task HandleMessageAsync(string message, object entity);
    }
}