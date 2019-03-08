using System;

namespace ChickenAPI.Packets.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketClientAttribute : Attribute
    {
        public PacketClientAttribute()
        {
        }

        /// <summary>
        /// Packet's header
        /// </summary>
        public string Header { get; set; }
    }
}