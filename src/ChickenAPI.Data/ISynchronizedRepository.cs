using System;
using ChickenAPI.Core.Data.TransferObjects;

namespace ChickenAPI.Core.Data.AccessLayer
{
    public interface ISynchronizedRepository<T> : ISynchronousRepository<T, Guid>, IAsyncRepository<T, Guid> where T : class, ISynchronizedDto
    {
    }
}