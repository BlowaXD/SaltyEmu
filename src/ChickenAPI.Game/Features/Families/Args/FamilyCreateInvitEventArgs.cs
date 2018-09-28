﻿using System;
using ChickenAPI.Game.Data.TransferObjects.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Families.Args
{
    public class FamilyCreateInvitEventArgs : ChickenEventArgs
    {
        public Guid Id { get; set; }
        public DateTime InvitationCreationTime { get; set; }

        public IPlayerEntity Sender { get; set; }
        public FamilyDto DestinationFamily { get; set; }

        public IPlayerEntity Target { get; set; }

        public bool Force { get; set; }
    }
}