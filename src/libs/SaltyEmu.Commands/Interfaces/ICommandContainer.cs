using System.Threading.Tasks;
using Qmmands;
using SaltyEmu.Commands.Entities;

namespace SaltyEmu.Commands.Interfaces
{
    public interface ICommandContainer
    {
        /// <summary>
        /// Asynchronously add a module to CommandContainer.
        /// </summary>
        Task AddModuleAsync<T>() where T : SaltyModuleBase;

        /// <summary>
        /// Adds a typeparser to the CommandContainer.
        /// </summary>
        /// <param name="typeParser">Instance of the TypeParser to add.</param>
        void AddTypeParser<T>(TypeParser<T> typeParser);

        /// <summary>
        /// Method which will parse the message and try to execute the command.
        /// </summary>
        /// <param name="message">Raw message to parse.</param>
        /// <param name="entity">Entity that sent the message.</param>
        Task HandleMessageAsync(string message, object entity);
    }
}