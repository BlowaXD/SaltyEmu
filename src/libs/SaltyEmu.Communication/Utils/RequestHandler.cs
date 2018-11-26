using System;
using System.Collections.Generic;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;

namespace SaltyEmu.Communication.Utils
{
    public class RequestHandler : IIpcRequestHandler
    {
        private static readonly Logger Log = Logger.GetLogger<IIpcRequestHandler>();
        private readonly Dictionary<Type, Action<IIpcRequest>> _packetHandlers = new Dictionary<Type, Action<IIpcRequest>>();

        public void Register<T>(Action<T> handler) where T : IIpcRequest
        {
            if (_packetHandlers.ContainsKey(typeof(T)))
            {
                return;
            }

            _packetHandlers.Add(typeof(T), handler as Action<IIpcRequest>);
        }

        public void Handle(IIpcRequest request, Type type)
        {
            if (!_packetHandlers.TryGetValue(type, out Action<IIpcRequest> handler))
            {
                return;
            }

            Log.Info($"Handling {type.Name} packet");
            handler.Invoke(request);
        }

        public void Handle<T>(T request) where T : IIpcRequest
        {
            Handle(request, typeof(T));
        }
    }
}