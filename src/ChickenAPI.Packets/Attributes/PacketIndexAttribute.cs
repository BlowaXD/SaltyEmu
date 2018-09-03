using System;
using System.Collections.Generic;

namespace ChickenAPI.Packets.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PacketIndexAttribute : Attribute
    {
        #region Instantiation

        /// <inheritdoc />
        /// <summary> Specify the Index of the packetBase to parse this property to. </summary>
        /// <param name="index">
        ///     The zero based index starting from header (exclusive).
        /// </param>
        /// <param name="isReturnPacket">
        ///     Adds an # to the Header and replaces Spaces with ^ if set to
        ///     true.
        /// </param>
        /// <param name="serializeToEnd">
        ///     Defines if everything from this index should
        ///     be serialized into the underlying property
        /// </param>
        /// <param name="removeSeparator">
        ///     Removes
        ///     the separator (.) for <see cref="T:System.Collections.Generic.List`1" /> packets.
        /// </param>
        /// <param name="separatorBeforeProperty"></param>
        /// <param name="separatorNestedElements"></param>
        public PacketIndexAttribute(int index, bool isReturnPacket = false, bool serializeToEnd = false, bool removeSeparator = false, string separatorBeforeProperty = " ",
            string separatorNestedElements = ".")
        {
            Index = index;
            IsReturnPacket = isReturnPacket;
            SerializeToEnd = serializeToEnd;
            RemoveSeparator = removeSeparator;
            SeparatorBeforeProperty = separatorBeforeProperty;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     The zero based index starting from the header (exclusive).
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     Adds an # to the Header and replaces Spaces with ^
        /// </summary>
        public bool IsReturnPacket { get; set; }

        /// <summary> Removes the separator (.) for <see cref="List{T}" /> packets. </summary>
        public bool RemoveSeparator { get; set; }

        /// <summary>
        ///     Defines if everything from this index should be serialized into the underlying property.
        /// </summary>
        public bool SerializeToEnd { get; set; }

        /// <summary>
        ///     Defines if this property will be added to the packet or not depending if there is a value
        /// </summary>
        public bool IsOptional { get; set; }

        /// <summary>
        ///     Defines the separator that will be used before property (by default : " ")
        /// </summary>
        public string SeparatorBeforeProperty { get; set; }

        /// <summary>
        ///     Defines the separator between every elements of an <see cref="IEnumerable{T}" />
        /// </summary>
        public string SeparatorNestedElements { get; set; }

        #endregion
    }
}