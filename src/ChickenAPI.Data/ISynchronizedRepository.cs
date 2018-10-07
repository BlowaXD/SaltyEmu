using System;

namespace ChickenAPI.Data
{
    public interface ISynchronizedRepository<T> : ISynchronousRepository<T, Guid>, IAsyncRepository<T, Guid> where T : class, ISynchronizedDto
    {
    }
}