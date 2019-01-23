using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Data;
using MongoDB.Driver;

namespace SaltyEmu.Database.MongoDB
{
    public abstract class MappedMongoRepository<TObject> : IMappedRepository<TObject> where TObject : class, IMappedDto
    {
        protected readonly IMongoDatabase Database;
        protected readonly IMongoCollection<TObject> Collection;

        protected MappedMongoRepository(MongoConfigurationBuilder builder) : this(builder.Build())
        {
        }

        protected MappedMongoRepository(MongoConfiguration conf)
        {
            var client = new MongoClient($"mongodb://{conf.Endpoint}:{conf.Port}");
            Database = client.GetDatabase(conf.DatabaseName);
            Collection = Database.GetCollection<TObject>(typeof(TObject).Name);
        }

        public async Task<IEnumerable<TObject>> GetAsync()
        {
            return await (await Collection.FindAsync(FilterDefinition<TObject>.Empty)).ToListAsync();
        }

        public async Task<TObject> GetByIdAsync(long id)
        {
            return await (await Collection.FindAsync(o => o.Id == id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TObject>> GetByIdsAsync(IEnumerable<long> ids)
        {
            return await (await Collection.FindAsync(o => ids.Any(s => s == o.Id))).ToListAsync();
        }

        public async Task<TObject> SaveAsync(TObject obj)
        {
            await Collection.FindOneAndReplaceAsync(o => o.Id == obj.Id, obj, new FindOneAndReplaceOptions<TObject, IMappedDto>
            {
                IsUpsert = true
            });

            return obj;
        }

        public async Task SaveAsync(IEnumerable<TObject> objs)
        {
            List<ReplaceOneModel<TObject>> bulks = objs.Select(obj =>
                new ReplaceOneModel<TObject>(Builders<TObject>.Filter.Where(s => s.Id == obj.Id), obj)
                    { IsUpsert = true }).ToList();

            await Collection.BulkWriteAsync(bulks, new BulkWriteOptions
            {
                IsOrdered = true
            });
        }

        public async Task DeleteByIdAsync(long id)
        {
            await Collection.DeleteOneAsync(obj => obj.Id == id);
        }

        public async Task DeleteByIdsAsync(IEnumerable<long> ids)
        {
            await Collection.DeleteManyAsync(o => ids.Any(id => o.Id == id));
        }

        public IEnumerable<TObject> Get()
        {
            return Collection.FindSync(FilterDefinition<TObject>.Empty).ToList();
        }

        public TObject GetById(long id)
        {
            return Collection.FindSync(o => o.Id == id).FirstOrDefault();
        }

        public IEnumerable<TObject> GetByIds(IEnumerable<long> ids)
        {
            return Collection.FindSync(o => ids.Any(id => id == o.Id)).ToList();
        }

        public TObject Save(TObject obj)
        {
            Collection.FindOneAndReplace(o => o.Id == obj.Id, obj, new FindOneAndReplaceOptions<TObject, IMappedDto>
            {
                IsUpsert = true
            });

            return obj;
        }

        public void Save(IEnumerable<TObject> objs)
        {
            List<ReplaceOneModel<TObject>> bulks = objs.Select(obj =>
                new ReplaceOneModel<TObject>(Builders<TObject>.Filter.Where(s => s.Id == obj.Id), obj)
                    { IsUpsert = true }).ToList();

            Collection.BulkWrite(bulks, new BulkWriteOptions
            {
                IsOrdered = true
            });
        }

        public void DeleteById(long id)
        {
            Collection.DeleteOne(o => o.Id == id);
        }

        public void DeleteByIds(IEnumerable<long> ids)
        {
            Collection.DeleteMany(s => ids.Any(id => id == s.Id));
        }
    }
}