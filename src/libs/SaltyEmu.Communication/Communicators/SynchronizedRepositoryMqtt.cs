using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Data;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Protocol.RepositoryPacket;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.Communication.Communicators
{
    public abstract class SynchronizedRepositoryMqtt<T> : ISynchronizedRepository<T> where T : class, ISynchronizedDto
    {
        private readonly IIpcClient _client;
        protected SynchronizedRepositoryMqtt(MqttClientConfigurationBuilder builder)
        {
        }

        public IEnumerable<T> Get()
        {
            return _client.RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = null }).ConfigureAwait(false).GetAwaiter().GetResult().Objects;
        }

        public T GetById(Guid id)
        {
            return _client.RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = new[] { id } }).ConfigureAwait(false).GetAwaiter().GetResult().Objects.ElementAt(0);
        }

        public IEnumerable<T> GetByIds(IEnumerable<Guid> ids)
        {
            return _client.RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = ids }).ConfigureAwait(false).GetAwaiter().GetResult().Objects;
        }

        public T Save(T obj)
        {
            return _client.RequestAsync<RepositorySaveRequest<T>.Response>(new RepositorySaveRequest<T> { Objects = new[] { obj } }).ConfigureAwait(false).GetAwaiter().GetResult().Objects.ElementAt(0);
        }

        public void Save(IEnumerable<T> objs)
        {
            _client.RequestAsync<RepositorySaveRequest<T>.Response>(new RepositorySaveRequest<T> { Objects = objs }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteById(Guid id)
        {
            _client.BroadcastAsync(new RepositoryDeleteRequest<Guid> { ObjectIds = new[] { id } }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void DeleteByIds(IEnumerable<Guid> ids)
        {
            _client.BroadcastAsync(new RepositoryDeleteRequest<Guid> { ObjectIds = ids }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return (await _client.RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = null })).Objects;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return (await _client.RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = new[] { id } })).Objects.ElementAt(0);
        }

        public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return (await _client.RequestAsync<RepositoryGetResponse<T>>(new RepositoryGetRequest<Guid> { ObjectIds = ids })).Objects;
        }

        public async Task<T> SaveAsync(T obj)
        {
            return (await _client.RequestAsync<RepositorySaveRequest<T>.Response>(new RepositorySaveRequest<T> { Objects = new[] { obj } })).Objects.ElementAt(0);
        }

        public async Task SaveAsync(IEnumerable<T> objs)
        {
            await _client.RequestAsync<RepositorySaveRequest<T>.Response>(new RepositorySaveRequest<T> { Objects = objs });
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await _client.BroadcastAsync(new RepositoryDeleteRequest<Guid> { ObjectIds = new[] { id } });
        }

        public async Task DeleteByIdsAsync(IEnumerable<Guid> ids)
        {
            await _client.BroadcastAsync(new RepositoryDeleteRequest<Guid> { ObjectIds = ids });
        }
    }
}