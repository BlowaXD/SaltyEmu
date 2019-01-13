using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChickenAPI.Core.Events
{
    public interface IEventPipeline
    {
        /// <summary>
        ///     Asynchronously send a notification to handlers of type T
        /// </summary>
        /// <param name="notification">Notification object</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>A task that represents the publish operation.</returns>
        Task Notify<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : IEventNotification;


        /// <summary>
        ///     Asynchronously registers a preprocessor in the pipeline for events of type <see cref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="preprocessor"></param>
        /// <returns></returns>
        Task RegisterPreprocessorAsync<T>(IEventPreprocessor preprocessor) where T : IEventNotification;

        /// <summary>
        ///     Asynchronously registers a filter in filters in piepline for event of the given type
        /// </summary>
        /// <param name="preprocessor"></param>
        /// <param name="type"></param>
        Task RegisterPreprocessorAsync(IEventPreprocessor preprocessor, Type type);

        /// <summary>
        ///     Asynchronously unregisters the preprocessor for handled type from the pipeline
        /// </summary>
        /// <param name="preprocessor"></param>
        /// <returns></returns>
        Task UnregisterPreprocessorAsync<T>(IEventPreprocessor preprocessor) where T : IEventNotification;

        /// <summary>
        ///     Asynchronously unregisters the preprocessor from the pipeline
        /// </summary>
        /// <param name="preprocessor"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task UnregisterPreprocessorAsync(IEventPreprocessor preprocessor, Type type);


        /// <summary>
        ///     Asynchronously registers a PostProcessor (aka Handler) in the pipeline for events of type <see cref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="postProcessor"></param>
        /// <param name="type">Type of event handled by the post processor</param>
        /// <returns></returns>
        Task RegisterPostProcessorAsync(IEventPostProcessor postProcessor, Type type);

        /// <summary>
        ///     Asynchronously registers a PostProcessor (aka Handler) in the pipeline for events of type <see cref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="postProcessor"></param>
        /// <returns></returns>
        Task RegisterPostProcessorAsync<T>(IEventPostProcessor postProcessor) where T : IEventNotification;

        /// <summary>
        ///     Asynchronously unregisters the postprocessor for handled type from the pipeline
        /// </summary>
        /// <param name="preprocessor"></param>
        /// <returns></returns>
        Task UnregisterPostprocessorAsync<T>(IEventPostProcessor preprocessor) where T : IEventNotification;

        /// <summary>
        ///     Asynchronously unregisters the postprocessor from the pipeline
        /// </summary>
        /// <param name="postProcessor"></param>
        /// <returns></returns>
        Task UnregisterPostprocessorAsync(IEventPostProcessor postProcessor, Type type);
    }
}