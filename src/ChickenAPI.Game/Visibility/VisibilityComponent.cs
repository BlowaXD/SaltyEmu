using System;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Utils;

namespace ChickenAPI.Game.Visibility
{
    public class VisibilityComponent : ComponentBase
    {
        public static event TypedSenderEventHandler<IEntity, VisibilityChangeArgs> VisibilityChange;

        public VisibilityComponent(IEntity entity) : base(entity) => Visibility = VisibilityType.Visible;

        public bool IsVisible => Visibility == VisibilityType.Visible;

        public bool IsInvisible => Visibility == VisibilityType.Invisible;

        public VisibilityType Visibility { get; private set; }

        public void ChangeVisibility(VisibilityType visible)
        {
            Visibility = visible;
            OnVisibilityChange(Entity, new VisibilityChangeArgs());
        }


        private static void OnVisibilityChange(IEntity sender, VisibilityChangeArgs e)
        {
            VisibilityChange?.Invoke(sender, e);
        }
    }

    public enum VisibilityType
    {
        Invisible,
        Visible,
    }

    public class VisibilityChangeArgs : EventArgs
    {
        public VisibilityType Visibility { get; set; }
    }
}