using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Packets
{
    public interface IBroadcastable
    {
        #region Packets

        /// <summary>
        /// Broadcast a packet to every entities in the context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packet"></param>
        void Broadcast<T>(T packet) where T : IPacket;

        /// <summary>
        /// Broadcast a packet to every entities in the context except
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="packet"></param>
        void Broadcast<T>(IPlayerEntity sender, T packet) where T : IPacket;

        #endregion
    }
}