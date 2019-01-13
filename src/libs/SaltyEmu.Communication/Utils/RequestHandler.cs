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
        private readonly Dictionary<Type, Func<IIpcPacket, Task>> _packetHandlers = new Dictionary<Type, Func<IIpcPacket, Task>>();
        private readonly Dictionary<Type, Func<IIpcRequest, Task>> _requestHandlers = new Dictionary<Type, Func<IIpcRequest, Task>>();

        public void Register<T>(Func<IIpcRequest, Task> handler) where T : IIpcRequest
        {
            if (_requestHandlers.ContainsKey(typeof(T)))
            {
                Log.Info($"{typeof(T)} is already handled");
                return;
            }

            _requestHandlers.Add(typeof(T), handler);
        }

        public void Register<T>(Func<IIpcPacket, Task> handler) where T : IIpcPacket
        {
            if (_packetHandlers.ContainsKey(typeof(T)))
            {
                Log.Info($"{typeof(T)} is already handled");
                return;
            }

            _packetHandlers.Add(typeof(T), handler);
        }

        public Task HandleAsync<T>(T request) where T : IIpcRequest
        {
            return HandleAsync(request, typeof(T));
        }

        public Task HandleAsync(IIpcRequest request, Type type)
        {
            if (!_requestHandlers.TryGetValue(type, out Func<IIpcRequest, Task> handler))
            {
                Log.Warn($"{type} could not be found !");
                return Task.CompletedTask;
            }

            Log.Info($"Handling {type.Name} request");
            handler(request);
            return Task.CompletedTask;
        }

        public Task HandleBroadcastAsync<T>(T packet) where T : IIpcPacket
        {
            return HandleBroadcastAsync(packet, typeof(T));
        }

        public Task HandleBroadcastAsync(IIpcPacket packet, Type type)
        {
            if (!_packetHandlers.TryGetValue(type, out Func<IIpcPacket, Task> handler))
            {
                Log.Warn($"{type} could not be found !");
                return Task.CompletedTask;
            }

            Log.Info($"Handling {type.Name} packet");
            handler(packet);
            return Task.CompletedTask;
        }
    }
}