using System;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Families.Args
{
    public class FamilyInvitAcceptEventArgs : ChickenEventArgs
    {
        public Guid InvitationId { get; set; }
        public IPlayerEntity Receiver { get; set; }
    }
}