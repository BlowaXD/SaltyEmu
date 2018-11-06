using System.Collections.Generic;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Network
{
    public interface IBroadcastable
    {
        /// <summary>
        /// Broadcasts the packet and receives the packet too
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packet"></param>
        void Broadcast<T>(T packet) where T : IPacket;

        /// <summary>
        /// Broadcast a packet to every entities in the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packet"></param>
        /// <param name="rule"></param>
        void Broadcast<T>(T packet, IBroadcastRule rule) where T : IPacket;

        /// <summary>
        /// Broadcasts all the packets and receives it as well
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packets"></param>
        void Broadcast<T>(IEnumerable<T> packets) where T : IPacket;

        /// <summary>
        /// Broadcasts given packets to every players in the same MapLayer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packets"></param>
        /// <param name="rule"></param>
        void Broadcast<T>(IEnumerable<T> packets, IBroadcastRule rule) where T : IPacket;


        /// <summary>
        /// Broadcasts given packets to every players in the same MapLayer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packets"></param>
        void Broadcast(IEnumerable<IPacket> packets);


        /// <summary>
        /// Broadcasts given packets to every players in the same MapLayer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packets"></param>
        /// <param name="rule"></param>
        void Broadcast(IEnumerable<IPacket> packets, IBroadcastRule rule);
    }
}