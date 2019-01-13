using System;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;

namespace ChickenAPI.Core.Events
{
    public abstract class GenericEventPostProcessorBase<TNotification> : IEventPostProcessor where TNotification : IEventNotification
    {
        protected readonly Logger Log = Logger.GetLogger($"[HANDLER:({typeof(TNotification)})]");
        public Type Type => typeof(TNotification);

        public Task Handle(IEventNotification notification, CancellationToken cancellation)
        {
            if (notification is TNotification typedNotification)
            {
                return Handle(typedNotification, cancellation);
            }

            return Task.CompletedTask;
        }

        protected abstract Task Handle(TNotification e, CancellationToken cancellation);
    }
}