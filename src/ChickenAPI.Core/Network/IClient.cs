using System.Collections.Generic;

namespace ChickenAPI.Core.Network
{
    public interface IClient<in TPacket>
    {
        /// <summary>
        ///     Send the packet to the client
        /// </summary>
        /// <param name="packet"></param>
        void SendPacket(TPacket packet);

        /// <summary>
        ///     Send the given parameters packets to the client
        /// </summary>
        /// <param name="packets"></param>
        void SendPackets(IEnumerable<TPacket> packets);

        /// <summary>
        /// </summary>
        void Disconnect();
    }
}