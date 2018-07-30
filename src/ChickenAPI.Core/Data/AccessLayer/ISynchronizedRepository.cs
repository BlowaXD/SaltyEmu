using System;
using ChickenAPI.Data.AccessLayer.Repository;

namespace ChickenAPI.Core.Data.AccessLayer
{
    public interface ISynchronizedRepository<T> : ISynchronousRepository<T, Guid>, IAsyncRepository<T, Guid> where T : class, ISynchronizedDto
    {
    }
}