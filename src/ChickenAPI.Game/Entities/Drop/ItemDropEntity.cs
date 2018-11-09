using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Entities.Drop
{
    public class ItemDropEntity : EntityBase
    {
        public ItemDropEntity(long id) : base(VisualType.MapObject, id)
        {
            // transport id should be its id
        }

        public ItemDto Item { get; set; }
        public long Quantity { get; set; }
        public bool IsQuest { get; set; }

        public override void Dispose()
        {
        }
    }
}