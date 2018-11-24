using System;
using System.Collections.Generic;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Entities.Drop
{
    public class ItemDropEntity : EntityBase, IDropEntity
    {
        public ItemDropEntity(long id) : base(VisualType.MapObject, id)
        {
            Components = new Dictionary<Type, IComponent>();
        }

        public ItemDto Item { get; set; }
        public bool IsQuestDrop { get; set; }
        public long Quantity { get; set; }
        public long ItemVnum { get; set; }
        public Position<short> Position { get; set; }
        public bool IsGold { get; set; }
        public DateTime DroppedTimeUtc { get; set; }

        public override void Dispose()
        {
        }
    }
}