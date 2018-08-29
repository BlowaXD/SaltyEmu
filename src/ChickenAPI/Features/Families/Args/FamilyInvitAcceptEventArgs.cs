using System;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Families.Args
{
    public class FamilyInvitAcceptEventArgs : ChickenEventArgs
    {
        public Guid InvitationId { get; set; }
        public IPlayerEntity Receiver { get; set; }
    }
}