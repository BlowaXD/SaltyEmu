namespace ChickenAPI.Data
{
    public interface IMappedRepository<T> : ISynchronousRepository<T, long>, IAsyncRepository<T, long> where T : class, IMappedDto
    {
    }
}