using ChickenAPI.Data.Item;

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

        public ItemInstanceDto Build() =>
            new ItemInstanceDto
            {
                Item = _item
            };

        public static implicit operator ItemInstanceDto(ItemInstanceBuilder builder) => builder.Build();
    }
}