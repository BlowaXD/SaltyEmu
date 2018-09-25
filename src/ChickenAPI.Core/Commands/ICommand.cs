using System.Collections.Generic;

namespace ChickenAPI.Core.Commands
{
    /// <summary>
    /// A command
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The command's header
        /// </summary>
        string Header { get; }

        string Content { get; }

        ICommandArgument[] Arguments { get; set; }
    }
}