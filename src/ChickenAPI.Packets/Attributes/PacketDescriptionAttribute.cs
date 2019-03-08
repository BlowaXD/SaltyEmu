using System;

namespace ChickenAPI.Packets.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketDescriptionAttribute : Attribute
    {
        public PacketDescriptionAttribute()
        {

        }

        /// <summary>
        /// Why this packet is sent and the different actions that are related to it
        /// </summary>
        public string Description { get; set; }
    }
}