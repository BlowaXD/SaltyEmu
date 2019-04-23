using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChickenAPI.Packets.Attributes;
using ChickenAPI.Packets.Old.Attributes;
using ChickenAPI.Packets.Old.Game.Server.Player;

namespace Toolkit.Commands
{
    public class DocumentationCommand
    {
        public static int Handle(DocumentationCommand command)
        {
            foreach (Type packet in typeof(SayPacket).Assembly.GetTypes().Where(s => s.GetCustomAttribute<PacketServerAttribute>() != null))
            {
                var header = packet.GetCustomAttribute<PacketServerAttribute>();
                var packetDesc = packet.GetCustomAttribute<PacketDescriptionAttribute>();
                List<PacketPropertyDescriptionAttribute> properties = new List<PacketPropertyDescriptionAttribute>();

                foreach (PropertyInfo property in packet.GetProperties().Where(info => info.GetCustomAttribute<PacketPropertyDescriptionAttribute>() != null))
                {
                    var propertyDesc = property.GetCustomAttribute<PacketPropertyDescriptionAttribute>();
                }
                // serialize packet to markdown file
            }

            return 0;
        }
    }
}