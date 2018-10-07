using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Data.AccessLayer.Item;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Character;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Character
{
    public class CharacterItemDao : SynchronizedRepositoryBase<ItemInstanceDto, CharacterItemModel>, IItemInstanceService
    {
        public CharacterItemDao(SaltyDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<(long, ItemDto)> GetBaseInventory(AuthorityType authorityType) => null;

        public IEnumerable<ItemInstanceDto> GetWearByCharacterId(long id)
        {
            try
            {
                return DbSet.Where(s => s.CharacterId == id).AsEnumerable().Select(Mapper.Map<ItemInstanceDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_WEAR_BY_CHARACTER_ID]", e);
                return null;
            }
        }

        public async Task<IEnumerable<ItemInstanceDto>> GetWearByCharacterIdAsync(long characterId)
        {
            try
            {
                return (await DbSet.Where(s => s.CharacterId == characterId && s.Item.Type == InventoryType.Wear).ToArrayAsync()).Select(Mapper.Map<ItemInstanceDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_WEAR_BY_CHARACTER_ID]", e);
                return null;
            }
        }

        public IEnumerable<ItemInstanceDto> GetByCharacterId(long characterId)
        {
            try
            {
                return DbSet.Where(s => s.CharacterId == characterId).ToArray().Select(Mapper.Map<ItemInstanceDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_CHARACTER_ID]", e);
                return null;
            }
        }

        public async Task<IEnumerable<ItemInstanceDto>> GetByCharacterIdAsync(long characterId)
        {
            try
            {
                return (await DbSet.Where(s => s.CharacterId == characterId).ToArrayAsync()).Select(Mapper.Map<ItemInstanceDto>).ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_CHARACTER_ID]", e);
                return null;
            }
        }
    }
}