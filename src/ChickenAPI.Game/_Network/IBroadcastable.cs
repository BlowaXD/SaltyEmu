using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Packets;

namespace ChickenAPI.Game._Network
{
    public interface IBroadcastable
    {
        Task BroadcastAsync<T>(T packet) where T : IPacket;

        Task BroadcastAsync<T>(T packet, IBroadcastRule rule) where T : IPacket;

        Task BroadcastAsync<T>(IEnumerable<T> packets) where T : IPacket;

        Task BroadcastAsync<T>(IEnumerable<T> packets, IBroadcastRule rule) where T : IPacket;

        Task BroadcastAsync(IEnumerable<IPacket> packets);

        Task BroadcastAsync(IEnumerable<IPacket> packets, IBroadcastRule rule);
    }
}