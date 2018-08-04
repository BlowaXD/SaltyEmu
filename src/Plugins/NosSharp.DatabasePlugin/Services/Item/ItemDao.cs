using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.Data.TransferObjects.Item;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models;
using NosSharp.DatabasePlugin.Services.Base;

namespace NosSharp.DatabasePlugin.Services.Item
{
    public class ItemDao : MappedRepositoryBase<ItemDto, ItemModel>, IItemService
    {
        private readonly Dictionary<long, ItemDto> _items = new Dictionary<long, ItemDto>();

        public ItemDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override ItemDto GetById(long id)
        {
            if (_items.TryGetValue(id, out ItemDto value))
            {
                return value;
            }

            value = base.GetById(id);
            _items[id] = value;

            return value;
        }

        public override async Task<ItemDto> GetByIdAsync(long id)
        {
            if (_items.TryGetValue(id, out ItemDto value))
            {
                return value;
            }

            value = await base.GetByIdAsync(id);
            _items[id] = value;

            return value;
        }
    }
}