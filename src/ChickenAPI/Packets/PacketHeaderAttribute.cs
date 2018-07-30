using System;
using ChickenAPI.Enums;

namespace ChickenAPI.Packets
{
    public class PacketHeaderAttribute : Attribute
    {
        #region Instantiation

        public PacketHeaderAttribute(string identification)
        {
            Identification = identification;
            NeedCharacter = true;
        }

        public PacketHeaderAttribute(string identification, bool needCharacter)
        {
            Identification = identification;
            NeedCharacter = needCharacter;
        }

        public PacketHeaderAttribute(string identification, byte amount)
        {
            Identification = identification;
            Amount = amount;
        }

        public PacketHeaderAttribute(string identification, byte amount, bool needCharacter)
        {
            Identification = identification;
            Amount = amount;
            NeedCharacter = needCharacter;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Permission to handle the packetBase
        /// </summary>
        public AuthorityType Authority { get; set; }

        /// <summary>
        ///     Unique identification of the Packet
        /// </summary>
        public string Identification { get; set; }

        /// <summary>
        ///     Amount of tcp message to create the Packet
        /// </summary>
        public byte Amount { get; set; }

        public bool NeedCharacter { get; set; }

        #endregion
    }
}