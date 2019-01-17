using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;

namespace SaltyEmu.Communication.Utils
{
    public sealed class PacketHandlersContainer : IIpcPacketHandlersContainer
    {
        private static readonly Logger Log = Logger.GetLogger<PacketHandlersContainer>();
        private readonly Dictionary<Type, IIpcPacketHandler> _packetHandlers = new Dictionary<Type, IIpcPacketHandler>();

        public event EventHandler<Type> Registered;
        public event EventHandler<Type> Unregistered;

        public Task RegisterAsync(IIpcPacketHandler handler, Type type)
        {
            if (_packetHandlers.ContainsKey(type))
            {
                Log.Info($"{type} is already handled");
                return Task.CompletedTask;
            }

            Log.Warn($"HANDLING -> {type}");
            _packetHandlers.Add(type, handler);
            OnRegistered(type);
            return Task.CompletedTask;
        }

        public Task HandleAsync(IIpcPacket request, Type type)
        {
            if (!_packetHandlers.TryGetValue(type, out IIpcPacketHandler handler))
            {
                Log.Warn($"{type} could not be found !");
                return Task.CompletedTask;
            }

            Log.Info($"Handling {type.Name} request");
            return handler.Handle(request);
        }

        public Task UnregisterAsync(Type type)
        {
            if (_packetHandlers.ContainsKey(type))
            {
                OnUnregistered(type);
                _packetHandlers.Remove(type);
            }

            return Task.CompletedTask;
        }

        private void OnRegistered(Type e)
        {
            Registered?.Invoke(this, e);
        }

        private void OnUnregistered(Type e)
        {
            Unregistered?.Invoke(this, e);
        }
    }
}