using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Core.Logging;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using NosSharp.DatabasePlugin.Context;

namespace NosSharp.DatabasePlugin.Services.Base
{
    public class MappedRepositoryBase<TObject, TModel> : IMappedRepository<TObject> where TObject : class, IMappedDto where TModel : class, IMappedDto, new()
    {
        protected static readonly Logger Log = Logger.GetLogger<TObject>();
        protected readonly NosSharpContext Context;
        protected readonly DbSet<TModel> DbSet;
        protected readonly IMapper Mapper;

        public MappedRepositoryBase(NosSharpContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
            DbSet = context.Set<TModel>();
        }


        public IEnumerable<TObject> Get() => DbSet.ToArray().Select(Mapper.Map<TObject>);

        public virtual TObject GetById(long id)
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

        public virtual IEnumerable<TObject> GetByIds(IEnumerable<long> ids)
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
                    Log.Info($"Not found : {obj.Id}");
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
                Log.Warn($"{JsonConvert.SerializeObject(obj)}");
                Log.Error("[SAVE]", e);
                return null;
            }
        }

        public virtual void Save(IEnumerable<TObject> objs)
        {
            try
            {
                if (objs.All(s => s == null))
                {
                    return;
                }

                List<TModel> tmp = objs.Where(s => s != null).Select(Mapper.Map<TModel>).ToList();
                using (IDbContextTransaction transaction = Context.Database.BeginTransaction())
                {
                    Context.BulkInsertOrUpdate(tmp, new BulkConfig
                    {
                        PreserveInsertOrder = true,
                        SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity
                    });
                    transaction.Commit();
                }

                Log.Info($"[SAVE] {tmp.Count} {typeof(TObject).Name} saved");
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error("[SAVE]", e);
            }
        }

        public virtual void DeleteById(long id)
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

        public virtual void DeleteByIds(IEnumerable<long> ids)
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
                Log.Error("[DELETE]", e);
            }
        }

        public async Task<IEnumerable<TObject>> GetAsync() => (await DbSet.ToArrayAsync()).Select(Mapper.Map<TObject>);

        public virtual async Task<TObject> GetByIdAsync(long id)
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

        public virtual async Task<IEnumerable<TObject>> GetByIdsAsync(IEnumerable<long> ids)
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
                TModel model = DbSet.Find(obj.Id);
                if (model == null)
                {
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
                Log.Warn($"{JsonConvert.SerializeObject(obj)}");
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

        public virtual async Task DeleteByIdAsync(long id)
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

        public virtual async Task DeleteByIdsAsync(IEnumerable<long> ids)
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