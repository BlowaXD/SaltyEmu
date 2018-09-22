using System;
using Newtonsoft.Json;

namespace SaltyEmu.IpcPlugin.Protocol
{
    public class PacketContainer
    {
        public Type Type { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}