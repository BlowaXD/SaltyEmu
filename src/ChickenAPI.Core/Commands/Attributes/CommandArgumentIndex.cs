using System;

namespace ChickenAPI.Core.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandArgumentIndex : Attribute
    {
        public int Index { get; }

        public CommandArgumentIndex(int index)
        {
            Index = index;  
        }
    }
}