using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChickenAPI.Game.Locomotion.Events
{
    public class TransformationLocomotion : ChickenEventArgs
    {
        public ItemInstanceDto Item { get; set; }
    }
}