using System;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public abstract class ChickenEventArgs : EventArgs
    {
        public IEntity Sender { get; set; }
    }
}