using System;
using ChickenAPI.Data;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Groups
{
    public class GroupInvitDto : ISynchronizedDto
    {
        public IPlayerEntity Sender { get; set; }
        public IPlayerEntity Target { get; set; }

        public DateTime CreationUtc { get; set; }
        public Guid Id { get; set; }
    }
}