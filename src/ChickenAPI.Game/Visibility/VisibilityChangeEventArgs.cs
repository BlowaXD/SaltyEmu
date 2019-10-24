using System;
using ChickenAPI.Data.Enums.Game.Visibility;

namespace ChickenAPI.Game.Visibility
{
    public class VisibilityChangeEventArgs : EventArgs
    {
        public VisibilityType Visibility { get; set; }
    }
}