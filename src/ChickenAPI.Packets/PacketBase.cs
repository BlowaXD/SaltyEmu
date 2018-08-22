namespace ChickenAPI.Game.Packets
{
    public abstract class PacketBase : IPacket
    {
        #region Properties

        public string Header { get; set; }

        /// <summary>
        /// </summary>
        public string OriginalContent { get; set; }

        #endregion
    }
}