using ChickenAPI.Enums.Game.Visibility;

namespace ChickenAPI.Game.Visibility
{
    /// <summary>
    /// Every entities that has Visible capacity
    /// </summary>
    public interface IVisibleCapacity
    {
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