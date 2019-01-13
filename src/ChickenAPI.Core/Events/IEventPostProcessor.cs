using System.Threading;
using System.Threading.Tasks;

namespace ChickenAPI.Core.Events
{
    /// <summary>
    ///     Defines a handler for any type of notification
    /// </summary>
    public interface IEventPostProcessor
    {
        Task Handle(IEventNotification notification, CancellationToken cancellation);
    }
}