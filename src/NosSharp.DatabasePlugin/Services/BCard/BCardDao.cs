using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.AccessLayer.BCard;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.BCard;
using ChickenAPI.Enums.Game.BCard;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models.BCard;
using NosSharp.DatabasePlugin.Services.Base;

namespace NosSharp.DatabasePlugin.Services.BCard
{
    public class BCardDao : MappedRepositoryBase<BCardDto, BCardModel>, IBCardService
    {
        public BCardDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }

        private TObject SubSave<TObject, TModel>(DbSet<TModel> toInsert, TObject obj) where TObject : class, IMappedDto where TModel : class, IMappedDto
        {
            try
            {
                TModel model = toInsert.Find(obj.Id);
                if (model == null)
                {
                    Log.Info($"Not found : {obj.Id}");
                    model = toInsert.Add(Mapper.Map<TModel>(obj)).Entity;
                }
                else
                {
                    Context.Entry(model).CurrentValues.SetValues(obj);
                    Context.Entry(model).State = EntityState.Modified;
                }
                Context.SaveChanges();
                return Mapper.Map<TObject>(model);
            }
            catch (Exception e)
            {
                Log.Error("[SAVE]", e);
                return null;
            }
        }

        public override BCardDto Save(BCardDto obj)
        {
            switch (obj.RelationType)
            {
                case BCardRelationType.NpcMonster:
                    return SubSave(Context.NpcMonsterBCards, obj);
                case BCardRelationType.Item:
                    return SubSave(Context.ItemBCards, obj);
                case BCardRelationType.Skill:
                    return SubSave(Context.SkillBCards, obj);
                case BCardRelationType.Card:
                    return SubSave(Context.CardBCards, obj);
                case BCardRelationType.Global:
                    return base.Save(obj);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void SubSaveMultiple<TModel>(IList<TModel> objs) where TModel : BCardModel
        {
            try
            {
                using (IDbContextTransaction transaction = Context.Database.BeginTransaction())
                {
                    Context.BulkInsertOrUpdate(objs, new BulkConfig
                    {
                        PreserveInsertOrder = true
                    });
                    transaction.Commit();
                }

                Log.Info($"[SAVE] {objs.Count} {typeof(TModel).Name} saved");
            }
            catch (Exception e)
            {
                Log.Error("[SAVE]", e);
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
                        SubSaveMultiple(i.Select(Mapper.Map<NpcMonsterBCardModel>).ToList());
                        break;
                    case BCardRelationType.Item:
                        SubSaveMultiple(i.Select(Mapper.Map<ItemBCardModel>).ToList());
                        break;
                    case BCardRelationType.Skill:
                        SubSaveMultiple(i.Select(Mapper.Map<SkillBCardModel>).ToList());
                        break;
                    case BCardRelationType.Card:
                        SubSaveMultiple(i.Select(Mapper.Map<CardBCardModel>).ToList());
                        break;
                    case BCardRelationType.Global:
                        base.Save(i);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private async Task<TObject> SubSaveAsync<TObject, TModel>(DbSet<TModel> toInsert, TObject obj) where TObject : class, IMappedDto where TModel : class, IMappedDto
        {
            try
            {
                var tmp = Mapper.Map<TModel>(obj);
                EntityEntry<TModel> lol = toInsert.Update(tmp);
                await Context.SaveChangesAsync();
                return Mapper.Map<TObject>(lol.Entity);
            }
            catch (Exception e)
            {
                Log.Error("[UPDATE]", e);
                return null;
            }
        }

        public override async Task<BCardDto> SaveAsync(BCardDto obj)
        {
            switch (obj.RelationType)
            {
                case BCardRelationType.NpcMonster:
                    return await SubSaveAsync(Context.NpcMonsterBCards, obj);
                case BCardRelationType.Item:
                    return await SubSaveAsync(Context.ItemBCards, obj);
                case BCardRelationType.Skill:
                    return await SubSaveAsync(Context.SkillBCards, obj);
                case BCardRelationType.Card:
                    return await SubSaveAsync(Context.CardBCards, obj);
                case BCardRelationType.Global:
                    return await base.SaveAsync(obj);
                default:
                    throw new ArgumentOutOfRangeException();
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
    }
}