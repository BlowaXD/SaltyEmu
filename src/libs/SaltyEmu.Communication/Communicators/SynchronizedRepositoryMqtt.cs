using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Protocol.RepositoryPacket;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.Communication.Communicators
{
    public abstract class SynchronizedRepositoryMqtt<T> : MqttIpcClient<T>, ISynchronizedRepository<T> where T : class, ISynchronizedDto
    {
        protected SynchronizedRepositoryMqtt(MqttClientConfigurationBuilder builder) : base(builder)
        {
        }

        public IEnumerable<T> Get()
        {
            return RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = null }).ConfigureAwait(false).GetAwaiter().GetResult().Objects;
        }

        public T GetById(Guid id)
        {
            return RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = new[] { id } }).ConfigureAwait(false).GetAwaiter().GetResult().Objects.ElementAt(0);
        }

        public IEnumerable<T> GetByIds(IEnumerable<Guid> ids)
        {
            return RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = ids }).ConfigureAwait(false).GetAwaiter().GetResult().Objects;
        }

        public T Save(T obj)
        {
            return RequestAsync<RepositorySaveResponse<T>>(new RepositorySaveRequest<T> { ObjectIds = new[] { obj } }).ConfigureAwait(false).GetAwaiter().GetResult().Objects.ElementAt(0);
        }

        public void Save(IEnumerable<T> objs)
        {
            RequestAsync<RepositorySaveResponse<T>>(new RepositorySaveRequest<T> { ObjectIds = objs }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteById(Guid id)
        {
            BroadcastAsync(new RepositoryDeleteRequest<Guid> { ObjectId = new[] { id } }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteByIds(IEnumerable<Guid> ids)
        {
            BroadcastAsync(new RepositoryDeleteRequest<Guid> { ObjectId = ids }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return (await RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = null })).Objects;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return (await RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = new[] { id } })).Objects.ElementAt(0);
        }

        public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return (await RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = ids })).Objects;
        }

        public async Task<T> SaveAsync(T obj)
        {
            return (await RequestAsync<RepositorySaveResponse<T>>(new RepositorySaveRequest<T> { ObjectIds = new[] { obj } })).Objects.ElementAt(0);
        }

        public async Task SaveAsync(IEnumerable<T> objs)
        {
            await RequestAsync<RepositorySaveResponse<T>>(new RepositorySaveRequest<T> { ObjectIds = objs });
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await BroadcastAsync(new RepositoryDeleteRequest<Guid> { ObjectId = new[] { id } });
        }

        public async Task DeleteByIdsAsync(IEnumerable<Guid> ids)
        {
            await BroadcastAsync(new RepositoryDeleteRequest<Guid> { ObjectId = ids });
        }
    }
}