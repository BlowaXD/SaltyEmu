using System;

namespace ChickenAPI.Core.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandArgumentDescriptionAttribute : Attribute
    {
        public CommandArgumentDescriptionAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }
}