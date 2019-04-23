﻿using System;

namespace ChickenAPI.Packets.Old.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PacketPropertyDescriptionAttribute : Attribute
    {
        public PacketPropertyDescriptionAttribute()
        {

        }

        /// <summary>
        /// Why is this property done for
        /// </summary>
        public string Description { get; set; }
    }
}