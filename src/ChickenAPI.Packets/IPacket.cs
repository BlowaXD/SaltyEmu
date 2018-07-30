using System;

namespace ChickenAPI.Packets
{
    public interface IPacket
    {
        /// <summary>
        /// 
        /// </summary>
        string Header { get; }

        /// <summary>
        /// <see cref="IPacket"/>
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string Serialize();

        /// <summary>
        /// Fills the properties within the packet from the buffers given in parameter
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="buffer"></param>
        void Deserialize(string[] buffer);
    }
}