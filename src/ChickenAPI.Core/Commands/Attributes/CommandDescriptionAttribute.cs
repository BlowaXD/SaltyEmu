using System;

namespace ChickenAPI.Core.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandDescriptionAttribute : Attribute
    {
        public CommandDescriptionAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }
}