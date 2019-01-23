using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;

namespace ChickenAPI.Core.Events
{
    public class BasicEventPipelineAsync : IEventPipeline
    {
        private static readonly Logger Log = Logger.GetLogger<BasicEventPipelineAsync>();
        private readonly Dictionary<Type, List<IEventPostProcessor>> _postprocessorsDictionary = new Dictionary<Type, List<IEventPostProcessor>>();
        private readonly Dictionary<Type, List<IEventPreprocessor>> _preprocessorsDictionary = new Dictionary<Type, List<IEventPreprocessor>>();

        public async Task Notify<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : IEventNotification
        {
            if (!_postprocessorsDictionary.TryGetValue(typeof(TNotification), out List<IEventPostProcessor> processors))
            {
                return;
            }

            if (!await CanSendEvent(notification, typeof(TNotification), cancellationToken))
            {
                return;
            }

            foreach (IEventPostProcessor postProcessor in processors)
            {
                try
                {
                    await postProcessor.Handle(notification, cancellationToken);
                }
                catch (Exception e)
                {
                    Log.Error("Notify()", e);
                }
            }
        }

        public Task RegisterPreprocessorAsync<T>(IEventPreprocessor preprocessor) where T : IEventNotification => RegisterPreprocessorAsync(preprocessor, typeof(T));

        public Task RegisterPreprocessorAsync(IEventPreprocessor preprocessor, Type type)
        {
            if (!_preprocessorsDictionary.TryGetValue(type, out List<IEventPreprocessor> handlers))
            {
                handlers = new List<IEventPreprocessor>();
                _preprocessorsDictionary[type] = handlers;
            }

            handlers.Add(preprocessor);
            return Task.CompletedTask;
        }

        public Task UnregisterPreprocessorAsync<T>(IEventPreprocessor preprocessor) where T : IEventNotification => UnregisterPreprocessorAsync(preprocessor, typeof(T));

        public Task UnregisterPreprocessorAsync(IEventPreprocessor preprocessor, Type type) => Task.CompletedTask;

        public Task RegisterPostProcessorAsync(IEventPostProcessor postProcessor, Type type)
        {
            if (!_postprocessorsDictionary.TryGetValue(type, out List<IEventPostProcessor> handlers))
            {
                handlers = new List<IEventPostProcessor>();
                _postprocessorsDictionary[type] = handlers;
            }

            handlers.Add(postProcessor);
            return Task.CompletedTask;
        }

        public Task RegisterPostProcessorAsync<T>(IEventPostProcessor postProcessor) where T : IEventNotification => RegisterPostProcessorAsync(postProcessor, typeof(T));

        public Task UnregisterPostprocessorAsync<T>(IEventPostProcessor preprocessor) where T : IEventNotification => UnregisterPostprocessorAsync(preprocessor, typeof(T));

        public Task UnregisterPostprocessorAsync(IEventPostProcessor postProcessor, Type type) => Task.CompletedTask;


        private async Task<bool> CanSendEvent(IEventNotification e, Type type, CancellationToken cancellationToken)
        {
            if (!_preprocessorsDictionary.TryGetValue(type, out List<IEventPreprocessor> filters))
            {
                return true;
            }

            foreach (IEventPreprocessor filter in filters)
            {
                // filter is not passed correctly
                if (await filter.Handle(e, cancellationToken) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}