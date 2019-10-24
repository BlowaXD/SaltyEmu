using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data;
using MongoDB.Driver;

namespace SaltyEmu.Database.MongoDB
{
    public abstract class SynchronizedMongoRepository<TObject> : ISynchronizedRepository<TObject> where TObject : class, ISynchronizedDto
    {
        protected readonly ILogger Log;
        protected readonly IMongoDatabase Database;
        protected readonly IMongoCollection<TObject> Collection;

        protected SynchronizedMongoRepository(MongoConfigurationBuilder builder, ILogger log) : this(builder.Build(), log)
        {
        }

        private SynchronizedMongoRepository(MongoConfiguration conf, ILogger log)
        {
            Log = log;
            MongoClientSettings settings = MongoClientSettings.FromConnectionString($"mongodb://{conf.Endpoint}:{conf.Port}");
            var client = new MongoClient(settings);
            client.Cluster.Initialize();
            List<string> tmp = client.ListDatabaseNames().ToList();
            foreach (string s in tmp)
            {
                Log.Info(s);
            }

            Database = client.GetDatabase(conf.DatabaseName);
            Collection = Database.GetCollection<TObject>(typeof(TObject).Name);
            Log.Info($"Connected to {conf.Endpoint}:{conf.Port}");
        }

        public async Task<IEnumerable<TObject>> GetAsync()
        {
            return await (await Collection.FindAsync(FilterDefinition<TObject>.Empty)).ToListAsync();
        }

        public async Task<TObject> GetByIdAsync(Guid id)
        {
            return await (await Collection.FindAsync(o => o.Id == id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TObject>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return await (await Collection.FindAsync(o => ids.Any(s => s == o.Id))).ToListAsync();
        }

        public async Task<TObject> SaveAsync(TObject obj)
        {
            return await Collection.FindOneAndUpdateAsync(o => o.Id == obj.Id, new ObjectUpdateDefinition<TObject>(obj));
        }

        public async Task SaveAsync(IEnumerable<TObject> objs)
        {
            // probably faster than FindOneAndUpdateOne for each object
            await Collection.DeleteManyAsync(o => objs.Any(s => s.Id == o.Id));
            await Collection.InsertManyAsync(objs);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await Collection.DeleteOneAsync(obj => obj.Id == id);
        }

        public async Task DeleteByIdsAsync(IEnumerable<Guid> ids)
        {
            await Collection.DeleteManyAsync(o => ids.Any(id => o.Id == id));
        }

        public IEnumerable<TObject> Get()
        {
            return Collection.FindSync(FilterDefinition<TObject>.Empty).ToList();
        }

        public TObject GetById(Guid id)
        {
            return Collection.FindSync(o => o.Id == id).FirstOrDefault();
        }

        public IEnumerable<TObject> GetByIds(IEnumerable<Guid> ids)
        {
            return Collection.FindSync(o => ids.Any(id => id == o.Id)).ToList();
        }

        public TObject Save(TObject obj)
        {
            TObject tmp = Collection.FindOneAndUpdate(o => o.Id == obj.Id, new ObjectUpdateDefinition<TObject>(obj));
            if (tmp == null)
            {
                Collection.InsertOne(obj);
            }

            return obj;
        }

        public void Save(IEnumerable<TObject> objs)
        {
            var ids = objs.Select(obj => obj.Id);
            foreach (var i in ids)
            {
                Collection.DeleteOne(s => s.Id == i);
            }

            Collection.InsertMany(objs);
        }

        public void DeleteById(Guid id)
        {
            Collection.DeleteOne(o => o.Id == id);
        }

        public void DeleteByIds(IEnumerable<Guid> ids)
        {
            Collection.DeleteMany(s => ids.Any(id => id == s.Id));
        }
    }
}