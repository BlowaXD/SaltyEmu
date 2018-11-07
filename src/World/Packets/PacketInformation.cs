using System;
using System.Collections.Generic;
using System.Reflection;
using ChickenAPI.Packets.Attributes;

namespace World.Packets
{
    public class PacketInformation
    {
        public string Header { get; set; }
        public Type Type { get; set; }

        public PacketPropertyContainer[] PacketProps { get; set; }
    }
}