using System;

namespace ChickenAPI.Packets.Old.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketServerAttribute : Attribute
    {
        public PacketServerAttribute()
        {
        }

        /// <summary>
        /// Packet's header
        /// </summary>
        public string Header { get; set; }
    }
}