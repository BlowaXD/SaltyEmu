using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Data.AccessLayer.Item;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Item;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Item
{
    public class ItemDao : MappedRepositoryBase<ItemDto, ItemModel>, IItemService
    {
        private readonly Dictionary<long, ItemDto> _items = new Dictionary<long, ItemDto>();

        public ItemDao(DbContext context, IMapper mapper) : base(context, mapper)
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