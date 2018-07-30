using System;
using System.Collections.Generic;
using System.Reflection;

namespace ChickenAPI.Packets
{
    public class PacketInformation
    {
        public string Header { get; set; }
        public Type Type { get; set; }
        public Dictionary<PacketIndexAttribute, PropertyInfo> PacketProperties { get; set; }
    }
}