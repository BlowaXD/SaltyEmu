using Qmmands;
using SaltyEmu.Commands.Entities;
using System.Threading.Tasks;

namespace SaltyEmu.Commands.Interfaces
{
    public interface ICommandContainer
    {
        /// <summary>
        ///     Asynchronously adds a module to CommandContainer.
        /// </summary>
        Task AddModuleAsync<T>() where T : SaltyModuleBase;

        /// <summary>
        ///     Asynchronously removes a module from the CommandContainer.
        /// </summary>
        Task RemoveModuleAsync<T>() where T : SaltyModuleBase;

        /// <summary>
        ///     Returns a module by its name;
        /// </summary>
        /// <param name="name">Name of the module. If the module has a Name attribute, the name will be the value of that attribute. If it doesn't, the name is the class name.</param>
        /// <param name="caseSensitive">Case sensitive.</param>
        /// <returns></returns>
        Module[] GetModulesByName(string name, bool caseSensitive = true);

        /// <summary>
        ///     Returns a command by its name.
        /// </summary>
        /// <param name="name">Name of the command.</param>
        /// <param name="caseSensitive">Case sensitive.</param>
        /// <returns></returns>
        Command[] GetCommandsByName(string name, bool caseSensitive = true);

        /// <summary>
        ///     Removes a command by its name.
        /// </summary>
        /// <param name="commandName">Name of the command.</param>
        /// <param name="caseSensitive">Case sensitive.</param>
        /// <returns></returns>
        Task RemoveCommandsAsync(string commandName, bool caseSensitive = true);

        /// <summary>
        ///     Removes the specified commands from the CommandContainer. 
        ///     As it doesn't internaly exist, we will find the module of this command and rebuilt it without that command.
        /// </summary>
        /// <param name="command">Commands to remove.</param>
        /// <returns></returns>
        Task RemoveCommandsAsync(Command[] command);

        /// <summary>
        ///     Removes the specified command from the CommandContainer. 
        ///     As it doesn't internaly exist, we will find the module of this command and rebuilt it without that command.
        /// </summary>
        /// <param name="command">Command to remove.</param>
        /// <returns></returns>
        Task RemoveCommandAsync(Command command);

        /// <summary>
        ///     Adds a typeparser to the CommandContainer.
        /// </summary>
        /// <param name="typeParser">Instance of the TypeParser to add.</param>
        void AddTypeParser<T>(TypeParser<T> typeParser);

        /// <summary>
        ///     Method which will parse the message and try to execute the command.
        /// </summary>
        /// <param name="message">Raw message to parse.</param>
        /// <param name="entity">Entity that sent the message.</param>
        Task HandleMessageAsync(string message, object entity);
    }
}