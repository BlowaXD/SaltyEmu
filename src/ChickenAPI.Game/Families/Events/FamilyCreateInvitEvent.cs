using System;
using ChickenAPI.Data.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Families.Events
{
    public class FamilyCreateInvitEvent : GameEntityEvent
    {
        public Guid Id { get; set; }
        public DateTime InvitationCreationTime { get; set; }

        public FamilyDto DestinationFamily { get; set; }

        public IPlayerEntity Target { get; set; }

        public bool Force { get; set; }
    }
}