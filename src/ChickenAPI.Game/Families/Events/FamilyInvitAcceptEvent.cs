using System;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Families.Events
{
    public class FamilyInvitAcceptEvent : GameEntityEvent
    {
        public Guid InvitationId { get; set; }
        public IPlayerEntity Receiver { get; set; }
    }
}