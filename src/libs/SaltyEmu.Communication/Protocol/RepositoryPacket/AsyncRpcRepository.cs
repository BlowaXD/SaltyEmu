using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data;

namespace SaltyEmu.Communication.Protocol.RepositoryPacket
{
    public class AsyncRpcRepository<T, TKey> where T : class
    {
        private readonly Logger _log = Logger.GetLogger<T>();
        private readonly IAsyncRepository<T, TKey> _repository;

        public AsyncRpcRepository(IAsyncRepository<T, TKey> repository, IIpcPacketHandlersContainer handler)
        {
            _repository = repository;
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
            await request.ReplyAsync(new RepositorySaveRequest<T>.Response
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