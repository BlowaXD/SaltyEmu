using System;
using ChickenAPI.Enums.Game.Visibility;

namespace ChickenAPI.Game.Visibility
{
    public class VisibilityChangeEventArgs : EventArgs
    {
        public VisibilityType Visibility { get; set; }
    }
}