using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.BCard;

namespace ChickenAPI.Game.Builders
{
    public class ItemInstanceBuilder
    {
        private ItemDto _item;

        public ItemInstanceBuilder WithItem(ItemDto item)
        {
            _item = item;
            return this;
        }

        public ItemInstanceDto Build()
        {
            return new ItemInstanceDto
            {
                Item = _item
            };
        }

        public static implicit operator ItemInstanceDto(ItemInstanceBuilder builder)
        {
            return builder.Build();
        }
    }
}