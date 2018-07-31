using System;
using System.Collections.Generic;
using System.Text;

namespace ChickenAPI.Packets.Login.Server
{
    [PacketServer]
    public class NsTesTPacket : IPacket
    {
        public string Header => "NsTeST";
        public Type Type => typeof(NsTesTPacket);

        /// <summary>
        /// index 0
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// index 1
        /// </summary>
        public int SessionId { get; set; }

        /// <summary>
        /// index 2
        /// </summary>
        public List<NsTeStSubPacket> SubPacket { get; set; }

        public string Serialize()
        {
            var tmp = new StringBuilder();

            tmp.Append(Header);
            tmp.AppendFormat(" {0} {1} ", AccountName, SessionId);
            foreach (NsTeStSubPacket subpacket in SubPacket)
            {
                tmp.AppendFormat("{0}:{1}:{2}:{3}.{4}.{5}", subpacket.Host ?? "-1", subpacket.Port ?? -1, subpacket.Color ?? -1, subpacket.WorldCount, subpacket.WorldId, subpacket.Name);
            }

            tmp.Append("-1:-1:-1:10000.10000.1");

            return tmp.ToString();
        }

        public void Deserialize(string[] buffer)
        {
            throw new NotImplementedException();
        }

        #region SubPackets

        public class NsTeStSubPacket
        {
            /// <summary>
            /// Ip Address
            /// </summary>
            public string Host { get; set; }

            /// <summary>
            /// [PacketIndex(1, SpecialSeparator = ":")]
            /// </summary>
            public int? Port { get; set; }

            /// <summary>
            /// [PacketIndex(2, SpecialSeparator = ":")]
            /// </summary>
            public int? Color { get; set; }

            /// <summary>
            /// [PacketIndex(3, SpecialSeparator = ":")]
            /// </summary>
            public int WorldCount { get; set; }

            /// <summary>
            /// [PacketIndex(4)]
            /// </summary>
            public int WorldId { get; set; }

            /// <summary>
            /// [PacketIndex(5)]
            /// </summary>
            public string Name { get; set; }
        }

        #endregion
    }
}