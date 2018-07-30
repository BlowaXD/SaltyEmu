using ChickenAPI.Core.Data.TransferObjects;

namespace ChickenAPI.Core.Data.AccessLayer
{
    public interface IMappedRepository<T> : ISynchronousRepository<T, long>, IAsyncRepository<T, long> where T : class, IMappedDto
    {
    }
}