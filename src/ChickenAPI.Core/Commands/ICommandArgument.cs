using System;

namespace ChickenAPI.Core.Commands
{
    /// <summary>
    /// A command argument
    /// </summary>
    public interface ICommandArgument
    {
        string Name { get; }
        int Index { get; }
        string OriginalValue { get; }

        string Help();
    }
}