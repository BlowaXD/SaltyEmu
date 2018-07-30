using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ChickenAPI.Core.Logging;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Game.Server;
using World.Extensions;

namespace World.Packets
{
    /// <summary>
    ///     First version of a pluggable PacketFactory
    /// </summary>
    public class PluggablePacketFactory : IPacketFactory
    {
        private static readonly Logger Log = Logger.GetLogger<PluggablePacketFactory>();
        private readonly Dictionary<Type, PacketInformation> _deserializationInformations = new Dictionary<Type, PacketInformation>();
        private readonly Dictionary<string, Type> _packetByHeader = new Dictionary<string, Type>();
        private readonly Dictionary<Type, string> _packetByType = new Dictionary<Type, string>();

        public string Serialize<TPacket>(TPacket packet) where TPacket : IPacket => Serialize(packet, typeof(TPacket));

        public string Serialize(IPacket packet, Type type)
        {
            try
            {
                // load pregenerated serialization information
                PacketInformation serializationInformation = GetSerializationInformation(type);

                var builder = new StringBuilder();
                builder.Append(serializationInformation.Header);

                int lastIndex = 0;
                foreach (KeyValuePair<PacketIndexAttribute, PropertyInfo> packetBasePropertyInfo in serializationInformation.PacketProperties)
                {
                    // check if we need to add a non mapped values (pseudovalues)
                    if (packetBasePropertyInfo.Key.Index > lastIndex + 1)
                    {
                        int amountOfEmptyValuesToAdd = packetBasePropertyInfo.Key.Index - (lastIndex + 1);

                        for (int i = 0; i < amountOfEmptyValuesToAdd; i++)
                        {
                            builder.Append(" 0");
                        }
                    }

                    // add value for current configuration
                    builder.Append(SerializeValue(packetBasePropertyInfo.Value.PropertyType, packetBasePropertyInfo.Value.GetValue(packet),
                        packetBasePropertyInfo.Value.GetCustomAttributes<ValidationAttribute>(), packetBasePropertyInfo.Key));

                    // check if the value should be serialized to end
                    if (packetBasePropertyInfo.Key.SerializeToEnd)
                    {
                        // we reached the end
                        break;
                    }

                    // set new index
                    lastIndex = packetBasePropertyInfo.Key.Index;
                }

                return builder.ToString();
            }
            catch (Exception e)
            {
                Log.Error("Wrong Packet format", e);
                return string.Empty;
            }
        }

        public IPacket Deserialize(string packetContent, Type packetType, bool includesKeepAliveIdentity)
        {
            try
            {
                PacketInformation serializationInformation = GetSerializationInformation(packetType);
                var deserializedPacket = (PacketBase)packetType.CreateInstance();
                SetDeserializationInformations(deserializedPacket, packetContent, serializationInformation.Header);
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
                return packetIndexAttribute?.SeparatorBeforeProperty + SerializeSimpleList((IList)value, propertyType);
            }

            return $"{packetIndexAttribute?.SeparatorBeforeProperty}{value}";
        }

        private string SerializeSimpleList(IList listValues, Type propertyType)
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
                    $".{SerializeValue(propertyType.GenericTypeArguments[0], listValues[i], propertyType.GenericTypeArguments[0].GetCustomAttributes<ValidationAttribute>()).Replace(" ", "")}";
            }

            return resultListPacket;
        }

        private string SerializeSubpacket(object value, PacketInformation subpacketSerializationInfo, bool isReturnPacket,
            bool shouldRemoveSeparator)
        {
            string serializedSubpacket = isReturnPacket ? $" #{subpacketSerializationInfo.Header}^" : " ";
            
            // iterate thru configure subpacket properties
            foreach (KeyValuePair<PacketIndexAttribute, PropertyInfo> subpacketPropertyInfo in subpacketSerializationInfo.PacketProperties)
            {
                // first element
                if (subpacketPropertyInfo.Key.Index != 0)
                {
                    serializedSubpacket += isReturnPacket ? "^" : shouldRemoveSeparator ? subpacketPropertyInfo.Key.SeparatorBeforeProperty : subpacketPropertyInfo.Key.SeparatorNestedElements;
                }

                serializedSubpacket += SerializeValue(subpacketPropertyInfo.Value.PropertyType, subpacketPropertyInfo.Value.GetValue(value),
                    subpacketPropertyInfo.Value.GetCustomAttributes<ValidationAttribute>(), subpacketPropertyInfo.Key).Replace(" ", "");
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

            foreach (PropertyInfo packetBasePropertyInfo in serializationType.GetProperties().Where(x => x.GetCustomAttributes(false).OfType<PacketIndexAttribute>().Any()))
            {
                PacketIndexAttribute indexAttribute = packetBasePropertyInfo.GetCustomAttributes(false).OfType<PacketIndexAttribute>().FirstOrDefault();

                if (indexAttribute != null)
                {
                    packetsForPacketDefinition.Add(indexAttribute, packetBasePropertyInfo);
                }
            }

            var tmp = new PacketInformation
            {
                Type = serializationType,
                Header = header,
                PacketProperties = packetsForPacketDefinition
            };
            _deserializationInformations.Add(serializationType, tmp);

            return tmp;
        }

        private PacketInformation GetSerializationInformation(Type serializationType) => _deserializationInformations.TryGetValue(serializationType, out PacketInformation packetInfo)
            ? packetInfo
            : GenerateSerializationInformations(serializationType);

        private string SerializeSubpackets(ICollection listValues, Type packetBasePropertyType, bool shouldRemoveSeparator)
        {
            string serializedSubPacket = string.Empty;
            PacketInformation subpacketSerializationInfo = GetSerializationInformation(packetBasePropertyType.GetGenericArguments()[0]);

            if (listValues.Count > 0)
            {
                serializedSubPacket = listValues.Cast<object>().Aggregate(serializedSubPacket,
                    (current, listValue) => current + SerializeSubpacket(listValue, subpacketSerializationInfo, false, shouldRemoveSeparator));
            }

            return serializedSubPacket;
        }

        #endregion


        #region Deserialization

        private PacketBase Deserialize(string packetContent, PacketBase deserializedPacketBase, PacketInformation serializationInformation, bool includesKeepAliveIdentity)
        {
            MatchCollection matches = Regex.Matches(packetContent, @"([^\040]+[\.][^\040]+[\040]?)+((?=\040)|$)|([^\040]+)((?=\040)|$)");
            if (matches.Count <= 0)
            {
                return deserializedPacketBase;
            }

            foreach (KeyValuePair<PacketIndexAttribute, PropertyInfo> packetBasePropertyInfo in serializationInformation.PacketProperties)
            {
                int currentIndex = packetBasePropertyInfo.Key.Index + (includesKeepAliveIdentity ? 2 : 1); // adding 2 because we need to skip incrementing number and packetBase header

                if (currentIndex >= matches.Count + (includesKeepAliveIdentity ? 1 : 0))
                {
                    break;
                }

                if (packetBasePropertyInfo.Key.SerializeToEnd)
                {
                    // get the value to the end and stop deserialization
                    int index = matches.Count > currentIndex ? matches[currentIndex].Index : packetContent.Length;
                    string valueToEnd = packetContent.Substring(packetContent.Length, packetContent.Length - index);
                    packetBasePropertyInfo.Value.SetValue(deserializedPacketBase,
                        DeserializeValue(packetBasePropertyInfo.Value.PropertyType, valueToEnd, packetBasePropertyInfo.Key, packetBasePropertyInfo.Value.GetCustomAttributes<ValidationAttribute>(),
                            matches, includesKeepAliveIdentity));
                    break;
                }

                string currentValue = matches[currentIndex].Value;

                // set the value & convert currentValue
                packetBasePropertyInfo.Value.SetValue(deserializedPacketBase,
                    DeserializeValue(packetBasePropertyInfo.Value.PropertyType,
                        currentValue,
                        packetBasePropertyInfo.Key,
                        packetBasePropertyInfo.Value.GetCustomAttributes<ValidationAttribute>(),
                        matches, includesKeepAliveIdentity
                    )
                );
            }

            return deserializedPacketBase;
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
            foreach (KeyValuePair<PacketIndexAttribute, PropertyInfo> subpacketPropertyInfo in subpacketSerializationInfo.PacketProperties)
            {
                int currentSubIndex = isReturnPacket ? subpacketPropertyInfo.Key.Index + 1 : subpacketPropertyInfo.Key.Index; // return packets do include header
                string currentSubValue = subpacketValues[currentSubIndex];

                subpacketPropertyInfo.Value.SetValue(newSubpacket,
                    DeserializeValue(subpacketPropertyInfo.Value.PropertyType, currentSubValue, subpacketPropertyInfo.Key, subpacketPropertyInfo.Value.GetCustomAttributes<ValidationAttribute>(),
                        null));
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
                int subPacketTypePropertiesCount = subpacketSerializationInfo.PacketProperties.Count;

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
            validationAttributes.ToList().ForEach(s =>
            {
                if (!s.IsValid(currentValue))
                {
                    throw new ValidationException(s.ErrorMessage);
                }
            });
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


        private void SetDeserializationInformations(PacketBase packetBaseDefinition, string packetContent, string packetHeader)
        {
            packetBaseDefinition.OriginalContent = packetContent;
            packetBaseDefinition.Header = packetHeader;
        }

        #endregion
    }
}