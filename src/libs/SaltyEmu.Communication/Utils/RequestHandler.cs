using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;

namespace SaltyEmu.Communication.Utils
{
    public class RequestHandler : IIpcRequestHandler
    {
        private static readonly Logger Log = Logger.GetLogger<IIpcRequestHandler>();
        private readonly Dictionary<Type, Func<IIpcRequest, Task>> _packetHandlers = new Dictionary<Type, Func<IIpcRequest, Task>>();

        public void Register<T>(Func<IIpcRequest, Task> handler) where T : IIpcRequest
        {
            if (_packetHandlers.ContainsKey(typeof(T)))
            {
                return;
            }

            _packetHandlers.Add(typeof(T), handler);
        }

        public Task HandleAsync<T>(T request) where T : IIpcRequest
        {
            return HandleAsync(request, typeof(T));
        }

        public async Task HandleAsync(IIpcRequest request, Type type)
        {
            if (!_packetHandlers.TryGetValue(type, out Func<IIpcRequest, Task> handler))
            {
                return;
            }

            await Task.Run(() => handler.Invoke(request));

            Log.Info($"Handling {type.Name} packet");
        }
    }
}