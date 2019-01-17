using System;

namespace SaltyEmu.Communication.Protocol.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PacketRequestAttribute : Attribute
    {
        public PacketRequestAttribute(Type prefix)
        {
            Topic = $"{prefix}/requests";
            ResponseTopic = $"{prefix}/responses";
        }

        public string Topic { get; }
        public string ResponseTopic { get; }
    }
}