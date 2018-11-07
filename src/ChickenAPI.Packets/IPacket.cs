using System;

namespace ChickenAPI.Packets
{
    public interface IPacket
    {
        /// <summary>
        /// Packet's sent date
        /// always set UTC time
        /// </summary>
        DateTime SentDateUtc { get; }

        /// <summary>
        /// Packet's header
        /// </summary>
        string Header { get; }

        /// <summary>
        /// Packet's content without its header
        /// </summary>
        string Content { get; }
    }
}