using System;

namespace ChickenAPI.Core.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandHeaderAttribute : Attribute
    {
        public CommandHeaderAttribute(string header)
        {
            Header = header;
        }

        public string Header { get; }
    }
}