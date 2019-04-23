using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ChickenAPI.Core.Logging;
using ChickenAPI.Packets.Attributes;
using ChickenAPI.Packets.Old;
using World.Extensions;
using PacketBase = ChickenAPI.Packets.PacketBase;

namespace World.Packets
{
    /// <summary>
    ///     First version of a pluggable PacketFactory
    /// </summary>
    public class PluggablePacketFactory : IPacketFactory
    {
        private static readonly Logger Log = Logger.GetLogger<PluggablePacketFactory>();
        private readonly Dictionary<string, Type> _packetTypesByHeader = new Dictionary<string, Type>();
        private readonly Dictionary<Type, PacketInformation> _deserializationInformations = new Dictionary<Type, PacketInformation>();


        public string Serialize(IPacket packet) => Serialize(packet, packet.GetType());

        public string Serialize<TPacket>(TPacket packet) where TPacket : IPacket =>
            Serialize(packet, typeof(TPacket));

        public string Serialize(IPacket packet, Type type)
        {
            try
            {
                // load pregenerated serialization information
                PacketInformation serializationInformation = GetSerializationInformation(type);

                var builder = new StringBuilder();
                builder.Append(serializationInformation.Header);

                int lastIndex = 0;

                foreach (PacketPropertyContainer property in serializationInformation.PacketProps)
                {
                    PacketIndexAttribute packetIndex = property.PacketIndex;
                    PropertyInfo propertyType = property.PropertyInfo;
                    IEnumerable<ValidationAttribute> validations = property.Validations;
                    // check if we need to add a non mapped values (pseudovalues)
                    if (packetIndex.Index > lastIndex + 1)
                    {
                        int amountOfEmptyValuesToAdd = packetIndex.Index - (lastIndex + 1);

                        for (int j = 0; j < amountOfEmptyValuesToAdd; j++)
                        {
                            builder.Append(" 0");
                        }
                    }

                    // add value for current configuration
                    builder.Append(SerializeValue(propertyType.PropertyType, propertyType.GetValue(packet), validations, packetIndex));

                    // check if the value should be serialized to end
                    if (packetIndex.SerializeToEnd)
                    {
                        // we reached the end
                        break;
                    }

                    // set new index
                    lastIndex = packetIndex.Index;
                }

                return builder.ToString();
            }
            catch (Exception e)
            {
                Log.Error($"Wrong Packet format {type}\n", e);
                return string.Empty;
            }
        }

        public IPacket Deserialize(string packetContent, Type packetType, bool includesKeepAliveIdentity)
        {
            try
            {
                PacketInformation serializationInformation = GetSerializationInformation(packetType);
                var deserializedPacket = (PacketBase)packetType.CreateInstance();
                SetDeserializationInformations(deserializedPacket, packetContent, serializationInformation.Header, packetType);
                return Deserialize(packetContent, deserializedPacket, serializationInformation, includesKeepAliveIdentity);
            }
            catch (Exception e)
            {
                Log.Error($"The serialized packetBase has the wrong format. Packet: {packetContent}", e);
                return null;
            }
        }

        #region Serialization

        private string SerializeValue(Type propertyType, object value, IEnumerable<ValidationAttribute> validationAttributes, PacketIndexAttribute packetIndexAttribute = null)
        {
            if (propertyType == null && validationAttributes.All(a => a.IsValid(value)))
            {
                return string.Empty;
            }

            if (packetIndexAttribute?.IsOptional == true && string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return string.Empty;
            }

            // check for nullable without value or string
            if (propertyType == typeof(string) && string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return $"{packetIndexAttribute?.SeparatorBeforeProperty}-";
            }

            if (Nullable.GetUnderlyingType(propertyType) != null && string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return $"{packetIndexAttribute?.SeparatorBeforeProperty}-1";
            }

            // enum should be casted to number
            if (propertyType.BaseType?.Equals(typeof(Enum)) == true)
            {
                return $"{packetIndexAttribute?.SeparatorBeforeProperty}{Convert.ToInt16(value)}";
            }

            if (propertyType == typeof(bool))
            {
                // bool is 0 or 1 not True or False
                return Convert.ToBoolean(value) ? $"{packetIndexAttribute?.SeparatorBeforeProperty}1" : $"{packetIndexAttribute?.SeparatorBeforeProperty}0";
            }

            if (value is PacketBase)
            {
                PacketInformation subpacketSerializationInfo = GetSerializationInformation(value.GetType());
                return SerializeSubpacket(value, subpacketSerializationInfo, packetIndexAttribute?.IsReturnPacket ?? false, packetIndexAttribute?.RemoveSeparator ?? false);
            }

            if (propertyType.BaseType?.Equals(typeof(PacketBase)) == true)
            {
                PacketInformation subpacketSerializationInfo = GetSerializationInformation(propertyType);
                return SerializeSubpacket(value, subpacketSerializationInfo, packetIndexAttribute?.IsReturnPacket ?? false, packetIndexAttribute?.RemoveSeparator ?? false);
            }

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))
                && propertyType.GenericTypeArguments[0].BaseType == typeof(PacketBase))
            {
                return packetIndexAttribute?.SeparatorBeforeProperty + SerializeSubpackets((IList)value, propertyType, packetIndexAttribute?.RemoveSeparator ?? false);
            }

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))) //simple list
            {
                return packetIndexAttribute?.SeparatorBeforeProperty + SerializeSimpleList((IList)value, propertyType, packetIndexAttribute);
            }

            return $"{packetIndexAttribute?.SeparatorBeforeProperty}{value}";
        }

        private string SerializeSimpleList(IList listValues, Type propertyType, PacketIndexAttribute index)
        {
            string resultListPacket = string.Empty;
            int listValueCount = listValues.Count;
            if (listValueCount <= 0)
            {
                return resultListPacket;
            }

            resultListPacket += SerializeValue(propertyType.GenericTypeArguments[0], listValues[0], propertyType.GenericTypeArguments[0].GetCustomAttributes<ValidationAttribute>());

            for (int i = 1; i < listValueCount; i++)
            {
                resultListPacket +=
                    $"{index.SeparatorNestedElements}{SerializeValue(propertyType.GenericTypeArguments[0], listValues[i], propertyType.GenericTypeArguments[0].GetCustomAttributes<ValidationAttribute>()).Replace(" ", "")}";
            }

            return resultListPacket;
        }

        private string SerializeSubpacket(object value, PacketInformation subpacketSerializationInfo, bool isReturnPacket,
            bool shouldRemoveSeparator)
        {
            string serializedSubpacket = isReturnPacket ? $" #{subpacketSerializationInfo.Header}^" : " ";

            // iterate thru configure subpacket properties
            foreach (PacketPropertyContainer tmp in subpacketSerializationInfo.PacketProps)
            {
                PacketIndexAttribute key = tmp.PacketIndex;
                PropertyInfo propertyInfo = tmp.PropertyInfo;
                // first element
                if (key.Index != 0)
                {
                    serializedSubpacket += isReturnPacket ? "^" : shouldRemoveSeparator ? key.SeparatorBeforeProperty : key.SeparatorNestedElements;
                }

                serializedSubpacket += SerializeValue(propertyInfo.PropertyType, propertyInfo.GetValue(value), tmp.Validations, key).Replace(" ", "");
            }

            return serializedSubpacket;
        }

        private PacketInformation GenerateSerializationInformations(Type serializationType)
        {
            string header = serializationType.GetCustomAttribute<PacketHeaderAttribute>()?.Identification;

            if (string.IsNullOrEmpty(header))
            {
                throw new Exception($"Packet header cannot be empty. PacketType: {serializationType.Name}");
            }

            Dictionary<PacketIndexAttribute, PropertyInfo> packetsForPacketDefinition = new Dictionary<PacketIndexAttribute, PropertyInfo>();

            IEnumerable<PropertyInfo> packetIndexProperties = serializationType.GetProperties().Where(x => x.GetCustomAttributes(false).OfType<PacketIndexAttribute>().Any()).ToArray();


            List<PacketPropertyContainer> packetProperties =
                (from packetBasePropertyInfo in packetIndexProperties.OrderBy(s => s.GetCustomAttribute<PacketIndexAttribute>(false).Index)
                 let indexAttribute = packetBasePropertyInfo.GetCustomAttributes(false).OfType<PacketIndexAttribute>().FirstOrDefault()
                 where indexAttribute != null
                 select new PacketPropertyContainer
                 {
                     PacketIndex = indexAttribute, PropertyInfo = packetBasePropertyInfo,
                     Validations = packetBasePropertyInfo.GetCustomAttributes<ValidationAttribute>()
                 }).ToList();

            var tmp = new PacketInformation
            {
                Type = serializationType,
                Header = header,
                PacketProps = packetProperties.OrderBy(container => container.PacketIndex.Index).ToArray()
            };

            if (!_packetTypesByHeader.ContainsKey(tmp.Header))
            {
                _packetTypesByHeader.Add(tmp.Header, serializationType);
            }

            _deserializationInformations.Add(serializationType, tmp);

            return tmp;
        }

        private PacketInformation GetSerializationInformation(Type serializationType)
        {
            if (_deserializationInformations.TryGetValue(serializationType, out PacketInformation packetInfo))
            {
                return packetInfo;
            }

            return GenerateSerializationInformations(serializationType);
        }

        private string SerializeSubpackets(ICollection listValues, Type packetBasePropertyType, bool shouldRemoveSeparator)
        {
            string serializedSubPacket = string.Empty;
            PacketInformation subpacketSerializationInfo = GetSerializationInformation(packetBasePropertyType.GetGenericArguments()[0]);

            if (listValues?.Count > 0)
            {
                serializedSubPacket = listValues.Cast<object>().Aggregate(serializedSubPacket,
                    (current, listValue) => current + SerializeSubpacket(listValue, subpacketSerializationInfo, false, shouldRemoveSeparator));
            }

            return serializedSubPacket;
        }

        #endregion


        #region Deserialization

        private PacketBase Deserialize(string packetContent, PacketBase packet, PacketInformation serializeInfos, bool includeKeepAlive)
        {
            MatchCollection matches = Regex.Matches(packetContent, @"([^\040]+[\.][^\040]+[\040]?)+((?=\040)|$)|([^\040]+)((?=\040)|$)");
            if (matches.Count <= 0)
            {
                return packet;
            }

            for (int i = 0; i < serializeInfos.PacketProps.Length; i++)
            {
                int currentIndex = i + (includeKeepAlive ? 2 : 1);
                if (currentIndex >= matches.Count)
                {
                    break;
                }

                PacketIndexAttribute index = serializeInfos.PacketProps[i].PacketIndex;
                PropertyInfo property = serializeInfos.PacketProps[i].PropertyInfo;
                IEnumerable<ValidationAttribute> validations = serializeInfos.PacketProps[i].Validations;

                if (index.SerializeToEnd)
                {
                    // get the value to the end and stop deserialization
                    int tmp = matches.Count > currentIndex ? matches[currentIndex].Index : packetContent.Length;
                    string valueToEnd = packetContent.Substring(tmp, packetContent.Length - tmp);
                    property.SetValue(packet, DeserializeValue(property.PropertyType, valueToEnd, index, validations, matches, includeKeepAlive));
                    break;
                }

                string currentValue = matches[currentIndex].Value;

                // set the value & convert currentValue
                property.SetValue(packet, DeserializeValue(property.PropertyType, currentValue, index, validations, matches, includeKeepAlive));
            }

            return packet;
        }

        private IList DeserializeSimpleList(string currentValues, Type genericListType)
        {
            var subpackets = (IList)Convert.ChangeType((IList)genericListType.CreateInstance(), genericListType);
            IEnumerable<string> splittedValues = currentValues.Split('.');

            foreach (string currentValue in splittedValues)
            {
                object value = DeserializeValue(genericListType.GenericTypeArguments[0], currentValue, null, genericListType.GenericTypeArguments[0].GetCustomAttributes<ValidationAttribute>(), null);
                subpackets.Add(value);
            }

            return subpackets;
        }

        private object DeserializeSubpacket(string currentSubValues, Type packetBasePropertyType, PacketInformation subpacketSerializationInfo, bool isReturnPacket = false)
        {
            string[] subpacketValues = currentSubValues.Split(isReturnPacket ? '^' : '.');
            object newSubpacket = packetBasePropertyType.CreateInstance();
            foreach (PacketPropertyContainer tmp in subpacketSerializationInfo.PacketProps)
            {
                PacketIndexAttribute key = tmp.PacketIndex;
                PropertyInfo value = tmp.PropertyInfo;
                int currentSubIndex = isReturnPacket ? key.Index + 1 : key.Index; // return packets do include header
                string currentSubValue = subpacketValues[currentSubIndex];

                value.SetValue(newSubpacket, DeserializeValue(value.PropertyType, currentSubValue, key, tmp.Validations, null));
            }

            return newSubpacket;
        }

        private IList DeserializeSubpackets(string currentValue, Type packetBasePropertyType, bool shouldRemoveSeparator, MatchCollection packetMatchCollections, int? currentIndex,
            bool includesKeepAliveIdentity)
        {
            // split into single values
            List<string> splittedSubpackets = currentValue.Split(' ').ToList();
            // generate new list
            var subpackets = (IList)Convert.ChangeType(packetBasePropertyType.CreateInstance(), packetBasePropertyType);

            Type subPacketType = packetBasePropertyType.GetGenericArguments()[0];
            PacketInformation subpacketSerializationInfo = GetSerializationInformation(subPacketType);

            // handle subpackets with separator
            if (shouldRemoveSeparator)
            {
                if (!currentIndex.HasValue || packetMatchCollections == null)
                {
                    return subpackets;
                }

                List<string> splittedSubpacketParts = packetMatchCollections.Select(m => m.Value).ToList();
                splittedSubpackets = new List<string>();

                string generatedPseudoDelimitedString = string.Empty;
                int subPacketTypePropertiesCount = subpacketSerializationInfo.PacketProps.Length;

                // check if the amount of properties can be serialized properly
                if (((splittedSubpacketParts.Count + (includesKeepAliveIdentity ? 1 : 0))
                    % subPacketTypePropertiesCount) == 0) // amount of properties per subpacket does match the given value amount in %
                {
                    for (int i = currentIndex.Value + 1 + (includesKeepAliveIdentity ? 1 : 0); i < splittedSubpacketParts.Count; i++)
                    {
                        int j;
                        for (j = i; j < i + subPacketTypePropertiesCount; j++)
                        {
                            // add delimited value
                            generatedPseudoDelimitedString += splittedSubpacketParts[j] + ".";
                        }

                        i = j - 1;

                        //remove last added separator
                        generatedPseudoDelimitedString = generatedPseudoDelimitedString.Substring(0, generatedPseudoDelimitedString.Length - 1);

                        // add delimited values to list of values to serialize
                        splittedSubpackets.Add(generatedPseudoDelimitedString);
                        generatedPseudoDelimitedString = string.Empty;
                    }
                }
                else
                {
                    throw new Exception("The amount of splitted subpacket values without delimiter do not match the % property amount of the serialized type.");
                }
            }

            foreach (string subpacket in splittedSubpackets)
            {
                subpackets.Add(DeserializeSubpacket(subpacket, subPacketType, subpacketSerializationInfo));
            }

            return subpackets;
        }

        private object DeserializeValue(Type packetPropertyType, string currentValue, PacketIndexAttribute packetIndexAttribute, IEnumerable<ValidationAttribute> validationAttributes,
            MatchCollection packetMatches,
            bool includesKeepAliveIdentity = false)
        {
            foreach (ValidationAttribute i in validationAttributes)
            {
                if (!i.IsValid(currentValue))
                {
                    throw new ValidationException(i.ErrorMessage);
                }
            }

            // check for empty value and cast it to null
            if (currentValue == "-1" || currentValue == "-")
            {
                currentValue = null;
            }

            // enum should be casted to number
            if (packetPropertyType.BaseType?.Equals(typeof(Enum)) == true)
            {
                object convertedValue = null;
                try
                {
                    if (currentValue != null && packetPropertyType.IsEnumDefined(Enum.Parse(packetPropertyType, currentValue)))
                    {
                        convertedValue = Enum.Parse(packetPropertyType, currentValue);
                    }
                }
                catch (Exception)
                {
                    Log.Warn($"Could not convert value {currentValue} to type {packetPropertyType.Name}");
                }

                return convertedValue;
            }

            if (packetPropertyType == typeof(bool)) // handle boolean values
            {
                return currentValue != "0";
            }

            if (packetPropertyType.BaseType?.Equals(typeof(PacketBase)) == true) // subpacket
            {
                PacketInformation subpacketSerializationInfo = GetSerializationInformation(packetPropertyType);
                return DeserializeSubpacket(currentValue, packetPropertyType, subpacketSerializationInfo, packetIndexAttribute?.IsReturnPacket ?? false);
            }

            if (packetPropertyType.IsGenericType && packetPropertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) // subpacket list
                && packetPropertyType.GenericTypeArguments[0].BaseType == typeof(PacketBase))
            {
                return DeserializeSubpackets(currentValue, packetPropertyType, packetIndexAttribute?.RemoveSeparator ?? false, packetMatches, packetIndexAttribute?.Index, includesKeepAliveIdentity);
            }

            if (packetPropertyType.IsGenericType && packetPropertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))) // simple list
            {
                return DeserializeSimpleList(currentValue, packetPropertyType);
            }

            if (Nullable.GetUnderlyingType(packetPropertyType) != null && string.IsNullOrEmpty(currentValue)) // empty nullable value
            {
                return null;
            }

            if (Nullable.GetUnderlyingType(packetPropertyType) != null) // nullable value
            {
                return packetPropertyType.GenericTypeArguments[0]?.BaseType == typeof(Enum)
                    ? Enum.Parse(packetPropertyType.GenericTypeArguments[0], currentValue)
                    : Convert.ChangeType(currentValue, packetPropertyType.GenericTypeArguments[0]);
            }

            if (packetPropertyType == typeof(string) && string.IsNullOrEmpty(currentValue) && !packetIndexAttribute.SerializeToEnd)
            {
                throw new NullReferenceException();
            }

            if (packetPropertyType == typeof(string) && currentValue == null)
            {
                currentValue = string.Empty;
            }

            return Convert.ChangeType(currentValue, packetPropertyType); // cast to specified type
        }


        private static void SetDeserializationInformations(PacketBase packetBaseDefinition, string packetContent, string packetHeader, Type packetType)
        {
            packetBaseDefinition.OriginalContent = packetContent;
            packetBaseDefinition.SentDateUtc = DateTime.UtcNow;
            packetBaseDefinition.Header = packetHeader;
            packetBaseDefinition.PacketType = packetType;
        }

        #endregion
    }
}