using System.Threading;
using System.Threading.Tasks;

namespace ChickenAPI.Core.Events
{
    public interface IEventPreprocessor
    {
        /// <summary>
        ///     Handles the preprocessor
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> Handle(IEventNotification notification, CancellationToken token);
    }
}