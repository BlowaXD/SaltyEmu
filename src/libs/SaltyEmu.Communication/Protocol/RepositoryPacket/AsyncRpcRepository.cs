using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Data;

namespace SaltyEmu.Communication.Protocol.RepositoryPacket
{
    public class AsyncRpcRepository<T, TKey> where T : class
    {
        private readonly IAsyncRepository<T, TKey> _repository;

        public AsyncRpcRepository(IAsyncRepository<T, TKey> repository, IIpcRequestHandler handler)
        {
            _repository = repository;
            // todo create delegates to remove the first parameter (which is supposed to be this)
            handler.Register<RepositoryGetRequest<TKey>>(OnGet);
            handler.Register<RepositorySaveRequest<T>>(OnSave);
            handler.Register<RepositoryDeleteRequest<TKey>>(OnDelete);
        }

        public async Task OnGet(IIpcRequest packet)
        {
            if (!(packet is RepositoryGetRequest<TKey> request))
            {
                return;
            }

            IEnumerable<T> tmp = await _repository.GetByIdsAsync(request.ObjectIds);
            await request.ReplyAsync(new RepositoryGetResponse<T>
            {
                Objects = tmp
            });
        }

        public async Task OnSave(IIpcRequest packet)
        {
            if (!(packet is RepositorySaveRequest<T> request))
            {
                return;
            }
            await _repository.SaveAsync(request.Objects);
            await request.ReplyAsync(new RepositorySaveResponse<T>
            {
                Objects = request.Objects
            });
        }

        public async Task OnDelete(IIpcRequest packet)
        {
            if (!(packet is RepositoryDeleteRequest<TKey> request))
            {
                return;
            }
            await _repository.DeleteByIdsAsync(request.ObjectIds);
        }
    }
}