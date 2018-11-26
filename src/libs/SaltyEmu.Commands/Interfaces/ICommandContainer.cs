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

        /// <summary>
        /// Adds a typeparser to the CommandContainer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeParser">Instance of the TypeParser to add.</param>
        void AddTypeParser<T>(TypeParser<T> typeParser);

        Task InitializeAsync();
        Task HandleMessageAsync(string message, object entity);
    }
}