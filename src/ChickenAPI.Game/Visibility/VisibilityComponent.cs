using System;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Visibility
{
    public class VisibilityComponent : ComponentBase, IVisibleCapacity
    {
        private VisibilityType _visibility;

        #region Events

        public event EventHandlerWithoutArgs<IVisibleEntity> Invisible;
        public event EventHandlerWithoutArgs<IVisibleEntity> Visible;

        #endregion

        #region Ctors

        public VisibilityComponent(IEntity entity) : base(entity) => Visibility = VisibilityType.Visible;

        #endregion

        #region Properties

        public bool IsVisible => Visibility == VisibilityType.Visible;

        public bool IsInvisible => Visibility == VisibilityType.Invisible;

        public VisibilityType Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                switch (value)
                {
                    case VisibilityType.Invisible:
                        Invisible?.Invoke(Entity as IVisibleEntity);
                        break;
                    case VisibilityType.Visible:
                        Visible?.Invoke(Entity as IVisibleEntity);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        #endregion
    }

    public class VisibilityChangeEventArgs : EventArgs
    {
        public VisibilityType Visibility { get; set; }
    }
}