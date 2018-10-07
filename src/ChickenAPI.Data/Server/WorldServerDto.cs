using System;
using ChickenAPI.Enums.Game.Character;

namespace ChickenAPI.Data.Server
{
    public class WorldServerDto
    {
        public Guid Id { get; set; }
        public short ChannelId { get; set; }
        public ChannelColor Color { get; set; }
        public string WorldGroup { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
    }
}