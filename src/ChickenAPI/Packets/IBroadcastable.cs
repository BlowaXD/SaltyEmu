using System.Collections.Generic;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Packets
{
    public interface IBroadcastable
    {
        #region Packets

        /// <summary>
        ///     Broadcast a packet to every entities in the context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packet"></param>
        void Broadcast<T>(T packet) where T : IPacket;

        void Broadcast<T>(IEnumerable<T> packets) where T : IPacket;

        void Broadcast(IEnumerable<IPacket> packets);

        /// <summary>
        ///     Broadcast a packet to every entities in the context except
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="packet"></param>
        void Broadcast<T>(IPlayerEntity sender, T packet) where T : IPacket;

        void Broadcast<T>(IPlayerEntity sender, IEnumerable<T> packets) where T : IPacket;

        #endregion
    }
}