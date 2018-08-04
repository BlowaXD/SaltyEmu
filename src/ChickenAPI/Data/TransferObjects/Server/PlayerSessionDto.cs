using System;
using ChickenAPI.Enums.Game;

namespace ChickenAPI.Game.Data.TransferObjects.Server
{
    public class PlayerSessionDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public PlayerSessionState State { get; set; }
        public Guid WorldServerId { get; set; }
    }
}