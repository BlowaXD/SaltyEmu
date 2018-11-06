using ChickenAPI.Data.Item;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Inventory
{
    public class InventoryComponent : IComponent
    {
        public const short MAX_ITEM_PER_SLOT = short.MaxValue;
        public const byte WEAR_SIZE = 16;
        public const byte EQUIPMENT_SIZE = 48;
        public const byte MAIN_SIZE = 48;
        public const byte ETC_SIZE = 48;

        public InventoryComponent(IEntity entity)
        {
            Wear = new ItemInstanceDto[WEAR_SIZE];
            Equipment = new ItemInstanceDto[EQUIPMENT_SIZE];
            Main = new ItemInstanceDto[MAIN_SIZE];
            Etc = new ItemInstanceDto[ETC_SIZE];
            // temporary
            Specialists = new ItemInstanceDto[1];
            Costumes = new ItemInstanceDto[1];
            Entity = entity;
        }

        public ItemInstanceDto[] Equipment { get; set; }

        public ItemInstanceDto[] Wear { get; set; }

        public ItemInstanceDto[] Main { get; set; }

        public ItemInstanceDto[] Etc { get; set; }

        public ItemInstanceDto[] Specialists { get; set; }

        public ItemInstanceDto[] Costumes { get; set; }

        public IEntity Entity { get; }
    }
}