using System;
using System.Threading.Tasks;

namespace ChickenAPI.Game.Events
{
    public abstract class GenericAsyncEventHandler<T> where T : ChickenEventArgs, IEventHandler
    {
        public Type HandledType => typeof(T);
        public abstract Task OnEventAsync(T e);
    }
}