using System;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Item;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Entities.Drop
{
    public interface IDropEntity : IEntity
    {
        Position<short> Position { get; set; }
        bool IsGold { get; set; }

        /// <summary>
        ///     Is the dropped item a drop
        /// </summary>
        bool IsQuestDrop { get; set; }

        /// <summary>
        ///     Dropped item quantity (to display)
        /// </summary>
        long Quantity { get; set; }

        /// <summary>
        ///     Itemvnum (to display)
        /// </summary>
        long ItemVnum { get; set; }

        /// <summary>
        ///     ItemInstance
        /// </summary>
        ItemDto Item { get; set; }

        /// <summary>
        ///     ItemInstance
        /// </summary>
        ItemInstanceDto ItemInstance { get; set; }

        /// <summary>
        ///     Dropped Item spawn time at UTC timezone
        /// </summary>
        DateTime DroppedTimeUtc { get; set; }
    }
}