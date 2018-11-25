using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Protocol.RepositoryPacket;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.Communication.Communicators
{
    public abstract class MappedRepositoryMqtt<T> : MqttIpcClient, IMappedRepository<T> where T : class, IMappedDto
    {
        protected MappedRepositoryMqtt(RabbitMqConfiguration config, IIpcSerializer serializer, string requestTopic, string responseTopic) : base(config, serializer, requestTopic, responseTopic)
        {
        }

        public IEnumerable<T> Get()
        {
            return RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<long> { ObjectIds = null }).ConfigureAwait(false).GetAwaiter().GetResult().Objects;
        }

        public T GetById(long id)
        {
            return RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<long> { ObjectIds = new[] { id } }).ConfigureAwait(false).GetAwaiter().GetResult().Objects.ElementAt(0);
        }

        public IEnumerable<T> GetByIds(IEnumerable<long> ids)
        {
            return RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<long> { ObjectIds = ids }).ConfigureAwait(false).GetAwaiter().GetResult().Objects;
        }

        public T Save(T obj)
        {
            return RequestAsync<RepositorySaveResponse<T>>(new RepositorySaveRequest<T> { ObjectIds = new T[] { obj } }).ConfigureAwait(false).GetAwaiter().GetResult().Objects.ElementAt(0);
        }

        public void Save(IEnumerable<T> objs)
        {
            RequestAsync<RepositorySaveResponse<T>>(new RepositorySaveRequest<T> { ObjectIds = objs }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteById(long id)
        {
            BroadcastAsync(new RepositoryDeleteRequest<long> { ObjectId = new[] { id } }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteByIds(IEnumerable<long> ids)
        {
            BroadcastAsync(new RepositoryDeleteRequest<long> { ObjectId = ids }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return (await RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<long> { ObjectIds = null })).Objects;
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return (await RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<long> { ObjectIds = new[] { id } })).Objects.ElementAt(0);
        }

        public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<long> ids)
        {
            return (await RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<long> { ObjectIds = ids })).Objects;
        }

        public async Task<T> SaveAsync(T obj)
        {
            return (await RequestAsync<RepositorySaveResponse<T>>(new RepositorySaveRequest<T> { ObjectIds = new[] { obj } })).Objects.ElementAt(0);
        }

        public async Task SaveAsync(IEnumerable<T> objs)
        {
            await RequestAsync<RepositorySaveResponse<T>>(new RepositorySaveRequest<T> { ObjectIds = objs });
        }

        public async Task DeleteByIdAsync(long id)
        {
            await BroadcastAsync(new RepositoryDeleteRequest<long> { ObjectId = new[] { id } });
        }

        public async Task DeleteByIdsAsync(IEnumerable<long> ids)
        {
            await BroadcastAsync(new RepositoryDeleteRequest<long> { ObjectId = ids });
        }
    }
}