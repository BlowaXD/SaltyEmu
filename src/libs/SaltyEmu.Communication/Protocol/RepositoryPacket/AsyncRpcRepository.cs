using System.Collections.Generic;
using ChickenAPI.Core.IPC;
using ChickenAPI.Data;

namespace SaltyEmu.Communication.Protocol.RepositoryPacket
{
    public class AsyncRpcRepository<T, TKey> where T : class
    {
        private readonly IAsyncRepository<T, TKey> _repository;

        public AsyncRpcRepository(IAsyncRepository<T, TKey> repository, IIpcRequestHandler handler)
        {
            _repository = repository;
            handler.Register<RepositoryGetRequest<TKey>>(OnMessage);
            handler.Register<RepositorySaveRequest<T>>(OnMessage);
            handler.Register<RepositoryDeleteRequest<TKey>>(OnMessage);
        }

        public async void OnMessage(RepositoryGetRequest<TKey> request)
        {
            IEnumerable<T> tmp = await _repository.GetByIdsAsync(request.ObjectIds);
            await request.ReplyAsync(new RepositoryGetResponse<T>
            {
                Objects = tmp
            });
        }

        public async void OnMessage(RepositorySaveRequest<T> request)
        {
            await _repository.SaveAsync(request.Objects);
            await request.ReplyAsync(new RepositorySaveResponse<T>
            {
                Objects = request.Objects
            });
        }

        public async void OnMessage(RepositoryDeleteRequest<TKey> request)
        {
            await _repository.DeleteByIdsAsync(request.ObjectIds);
        }
    }
}