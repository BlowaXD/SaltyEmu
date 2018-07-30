namespace ChickenAPI.Data.AccessLayer.Repository
{
    public interface IMappedRepository<T> : ISynchronousRepository<T, long>, IAsyncRepository<T, long> where T : class, IMappedDto
    {
    }
}