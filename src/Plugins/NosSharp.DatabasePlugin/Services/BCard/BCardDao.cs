using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data;
using ChickenAPI.Data.BCard;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Game.Data.AccessLayer.BCard;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.BCard;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.BCard
{
    public class BCardDao : MappedRepositoryBase<BCardDto, BCardModel>, IBCardService
    {
        public BCardDao(SaltyDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override BCardDto Save(BCardDto obj)
        {
            switch (obj.RelationType)
            {
                case BCardRelationType.NpcMonster:
                    return Save(Context.NpcMonsterBCards, obj);
                case BCardRelationType.Item:
                    return Save(Context.ItemBCards, obj);
                case BCardRelationType.Skill:
                    return Save(Context.SkillBCards, obj);
                case BCardRelationType.Card:
                    return Save(Context.CardBCards, obj);
                case BCardRelationType.Global:
                    return base.Save(obj);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Save(IEnumerable<BCardDto> objs)
        {
            IEnumerable<IGrouping<BCardRelationType, BCardDto>> tmp = objs.GroupBy(s => s.RelationType);

            foreach (IGrouping<BCardRelationType, BCardDto> i in tmp)
            {
                switch (i.Key)
                {
                    case BCardRelationType.NpcMonster:
                        Save(i.Select(Mapper.Map<NpcMonsterBCardModel>).ToList());
                        break;
                    case BCardRelationType.Item:
                        Save(i.Select(Mapper.Map<ItemBCardModel>).ToList());
                        break;
                    case BCardRelationType.Skill:
                        Save(i.Select(Mapper.Map<SkillBCardModel>).ToList());
                        break;
                    case BCardRelationType.Card:
                        Save(i.Select(Mapper.Map<CardBCardModel>).ToList());
                        break;
                    case BCardRelationType.Global:
                        base.Save(i);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override async Task<BCardDto> SaveAsync(BCardDto obj)
        {
            switch (obj.RelationType)
            {
                case BCardRelationType.NpcMonster:
                    return await SaveAsync(Context.NpcMonsterBCards, obj);
                case BCardRelationType.Item:
                    return await SaveAsync(Context.ItemBCards, obj);
                case BCardRelationType.Skill:
                    return await SaveAsync(Context.SkillBCards, obj);
                case BCardRelationType.Card:
                    return await SaveAsync(Context.CardBCards, obj);
                case BCardRelationType.Global:
                    return await base.SaveAsync(obj);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task<BCardDto> GetByIdAndType(long id, BCardRelationType type)
        {
            switch (type)
            {
                case BCardRelationType.NpcMonster:
                    return await SubGetByIdAsync<BCardDto, NpcMonsterBCardModel>(Context.NpcMonsterBCards, id);
                case BCardRelationType.Item:
                    return await SubGetByIdAsync<BCardDto, ItemBCardModel>(Context.ItemBCards, id);
                case BCardRelationType.Skill:
                    return await SubGetByIdAsync<BCardDto, SkillBCardModel>(Context.SkillBCards, id);
                case BCardRelationType.Card:
                    return await SubGetByIdAsync<BCardDto, CardBCardModel>(Context.CardBCards, id);
                case BCardRelationType.Global:
                    return await GetByIdAsync(id);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task<IEnumerable<BCardDto>> GetBySkillIdAsync(long skillId)
        {
            try
            {
                return (await Context.SkillBCards.Where(s => s.RelationId == skillId).ToListAsync()).Select(Mapper.Map<BCardDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_SKILL_ID]", e);
                return null;
            }
        }

        public async Task<IEnumerable<BCardDto>> GetByMapMonsterIdAsync(long monsterId)
        {
            try
            {
                return (await Context.NpcMonsterBCards.Where(s => s.RelationId == monsterId).ToListAsync()).Select(Mapper.Map<BCardDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_SKILL_ID]", e);
                return null;
            }
        }

        public async Task<IEnumerable<BCardDto>> GetByCardIdAsync(long cardId)
        {
            try
            {
                return (await Context.CardBCards.Where(s => s.RelationId == cardId).ToListAsync()).Select(Mapper.Map<BCardDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_SKILL_ID]", e);
                return null;
            }
        }

        public async Task<IEnumerable<BCardDto>> GetByItemIdAsync(long itemId)
        {
            try
            {
                return (await Context.ItemBCards.Where(s => s.RelationId == itemId).ToListAsync()).Select(Mapper.Map<BCardDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_ITEM_ID]", e);
                return null;
            }
        }
        public async Task<TObject> SubGetByIdAsync<TObject, TModel>(DbSet<TModel> dbSet, long id) where TModel : class where TObject : class
        {
            try
            {
                TModel lol = await dbSet.FindAsync(id);
                await Context.SaveChangesAsync();
                return Mapper.Map<TObject>(lol);
            }
            catch (Exception e)
            {
                Log.Error("[UPDATE]", e);
                return null;
            }
        }
    }
}