using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums.Game.Visibility;

namespace ChickenAPI.Game.Visibility
{
    /// <summary>
    /// ceci est une proposition d'interface pour remplacer la IVisibleEntity actuelle
    /// </summary>
    public interface IVisibleEntity
    {
        /// <summary>
        /// Triggered event when the entity becomes invisible
        /// </summary>
        event EventHandlerWithoutArgs<IVisibleEntity> Invisible;

        /// <summary>
        /// Triggered event when the entity becomes visible
        /// </summary>
        event EventHandlerWithoutArgs<IVisibleEntity> Visible;

        /// <summary>
        /// Tells if the entity is Visible
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Tells if the entity is Invisible
        /// </summary>
        bool IsInvisible { get; }

        /// <summary>
        /// Actual Visibility State
        /// </summary>
        VisibilityType Visibility { get; set; }
    }
}