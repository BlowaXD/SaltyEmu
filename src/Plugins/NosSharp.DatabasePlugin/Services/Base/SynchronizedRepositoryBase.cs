using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Core.Logging;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SaltyEmu.DatabasePlugin.Context;

namespace SaltyEmu.DatabasePlugin.Services.Base
{
    public class SynchronizedRepositoryBase<TObject, TModel> : ISynchronizedRepository<TObject>
    where TObject : class, ISynchronizedDto where TModel : class, ISynchronizedDto, new()
    {
        protected static readonly Logger Log = Logger.GetLogger<TModel>();
        protected readonly NosSharpContext Context;
        protected readonly DbSet<TModel> DbSet;
        protected readonly IMapper Mapper;

        public SynchronizedRepositoryBase(NosSharpContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
            DbSet = context.Set<TModel>();
        }

        public IEnumerable<TObject> Get() => DbSet.ToArray().Select(Mapper.Map<TObject>);

        public virtual TObject GetById(Guid id)
        {
            try
            {
                return Mapper.Map<TObject>(DbSet.Find(id));
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_ID]", e);
                return null;
            }
        }

        public virtual IEnumerable<TObject> GetByIds(IEnumerable<Guid> ids)
        {
            try
            {
                return DbSet.Where(s => ids.Contains(s.Id)).AsEnumerable().Select(Mapper.Map<TObject>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_IDS]", e);
                return null;
            }
        }

        public virtual TObject Save(TObject obj)
        {
            try
            {
                TModel model = DbSet.Find(obj.Id);
                if (model == null)
                {
                    if (obj.Id == Guid.Empty)
                    {
                        obj.Id = Guid.NewGuid();
                    }

                    model = DbSet.Add(Mapper.Map<TModel>(obj)).Entity;
                }
                else
                {
                    Context.Entry(model).CurrentValues.SetValues(obj);
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

        public virtual void Save(IEnumerable<TObject> objs)
        {
            try
            {
                IEnumerable<TObject> enumerable = objs as TObject[] ?? objs.ToArray();
                if (enumerable.All(s => s == null))
                {
                    return;
                }

                List<TModel> tmp = enumerable.Where(s => s != null).Select(Mapper.Map<TModel>).ToList();
                using (IDbContextTransaction transaction = Context.Database.BeginTransaction())
                {
                    Context.BulkInsertOrUpdate(tmp, new BulkConfig
                    {
                        PreserveInsertOrder = true
                    });
                    transaction.Commit();
                }

                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error("[SAVE]", e);
            }
        }

        public virtual void DeleteById(Guid id)
        {
            try
            {
                var model = new TModel { Id = id };
                DbSet.Attach(model);
                DbSet.Remove(model);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error("[DELETE_BY_ID]", e);
            }
        }

        public virtual void DeleteByIds(IEnumerable<Guid> ids)
        {
            try
            {
                List<TModel> tmp = DbSet.Where(f => ids.Contains(f.Id)).ToList();
                using (IDbContextTransaction transaction = Context.Database.BeginTransaction())
                {
                    Context.BulkDelete(tmp, new BulkConfig
                    {
                        PreserveInsertOrder = true
                    });
                    transaction.Commit();
                }

                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error("[DELETE_BY_IDS]", e);
            }
        }

        public async Task<IEnumerable<TObject>> GetAsync() => (await DbSet.ToArrayAsync()).Select(Mapper.Map<TObject>);

        public virtual async Task<TObject> GetByIdAsync(Guid id)
        {
            try
            {
                return Mapper.Map<TObject>(await DbSet.FindAsync(id));
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_ID]", e);
                return null;
            }
        }

        public virtual async Task<IEnumerable<TObject>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            try
            {
                return (await DbSet.Where(s => ids.Contains(s.Id)).ToListAsync()).Select(Mapper.Map<TObject>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_IDS]", e);
                return null;
            }
        }

        public virtual async Task<TObject> SaveAsync(TObject obj)
        {
            try
            {
                TModel model = await DbSet.FindAsync(obj.Id);
                if (model == null)
                {
                    if (obj.Id == Guid.Empty)
                    {
                        obj.Id = Guid.NewGuid();
                    }

                    model = DbSet.Add(Mapper.Map<TModel>(obj)).Entity;
                }
                else
                {
                    Context.Entry(model).CurrentValues.SetValues(obj);
                }

                await Context.SaveChangesAsync();
                return Mapper.Map<TObject>(model);
            }
            catch (Exception e)
            {
                Log.Error("[SAVE]", e);
                return null;
            }
        }

        public virtual async Task SaveAsync(IEnumerable<TObject> objs)
        {
            try
            {
                List<TModel> tmp = objs.Select(Mapper.Map<TModel>).ToList();
                using (IDbContextTransaction transaction = Context.Database.BeginTransaction())
                {
                    await Context.BulkInsertOrUpdateAsync(tmp, new BulkConfig
                    {
                        PreserveInsertOrder = true
                    });
                    transaction.Commit();
                }

                Context.SaveChanges();
                Log.Info($"[SAVE] {tmp.Count} {typeof(TObject).Name} saved");
            }
            catch (Exception e)
            {
                Log.Error("[SAVE]", e);
            }
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            try
            {
                var model = new TModel { Id = id };
                DbSet.Attach(model);
                DbSet.Remove(model);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error("[DELETE_BY_ID]", e);
            }
        }

        public async Task DeleteByIdsAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<TModel> tmp = DbSet.Where(f => ids.Contains(f.Id)).ToList();
                using (IDbContextTransaction transaction = Context.Database.BeginTransaction())
                {
                    await Context.BulkDeleteAsync(tmp, new BulkConfig
                    {
                        PreserveInsertOrder = true
                    });
                    transaction.Commit();
                }

                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error("[DELETE]", e);
            }
        }
    }
}