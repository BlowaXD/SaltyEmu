using System;
using System.Collections.Generic;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.IpcPlugin.Communicators
{
    public class RequestHandler : IIpcRequestHandler
    {
        private readonly Dictionary<Type, Action<IIpcRequest>> _packetHandlers = new Dictionary<Type, Action<IIpcRequest>>();

        public void Register<T>(Action<IIpcRequest> handler) where T : IIpcRequest
        {
            if (_packetHandlers.ContainsKey(typeof(T)))
            {
                return;
            }

            _packetHandlers.Add(typeof(T), handler);
        }

        public void Handle(IIpcRequest request, Type type)
        {
            if (!_packetHandlers.TryGetValue(type, out Action<IIpcRequest> handler))
            {
                return;
            }

            handler.Invoke(request);
        }

        public void Handle<T>(T request) where T : IIpcRequest
        {
            Handle(request, typeof(T));
        }
    }
}