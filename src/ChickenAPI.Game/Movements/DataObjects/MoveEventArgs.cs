using System;
using ChickenAPI.Core.Utils;

namespace ChickenAPI.Game.Movements.DataObjects
{
    public class MoveEventArgs : EventArgs
    {
        public MovableComponent Component { get; set; }
        public Position<short> Old { get; set; }
        public Position<short> New { get; set; }
    }
}