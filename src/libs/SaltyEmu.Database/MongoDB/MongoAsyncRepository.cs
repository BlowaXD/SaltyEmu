using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data;
using MongoDB.Driver;

namespace SaltyEmu.Database.MongoDB
{
    public class MongoAsyncRepository<TObject> : IMappedRepository<TObject> where TObject : class, IMappedDto
    {
        protected readonly IMongoDatabase Database;
        protected readonly IMongoCollection<TObject> Collection;

        public MongoAsyncRepository(string dbName)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            Database = client.GetDatabase(dbName);
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
            return await Collection.FindOneAndUpdateAsync(o => o.Id == obj.Id, new ObjectUpdateDefinition<TObject>(obj));
        }

        public async Task SaveAsync(IEnumerable<TObject> objs)
        {
            // probably faster than FindOneAndUpdateOne for each object
            await Collection.DeleteManyAsync(o => objs.Any(s => s.Id == o.Id));
            await Collection.InsertManyAsync(objs);
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
            return Collection.FindOneAndUpdate(o => o.Id == obj.Id, new ObjectUpdateDefinition<TObject>(obj));
        }

        public void Save(IEnumerable<TObject> objs)
        {
            Collection.DeleteMany(s => objs.Any(obj => s.Id == obj.Id));
            Collection.InsertMany(objs);
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