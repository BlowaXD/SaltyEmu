using System;
using System.Collections.Generic;
using System.Text;
using ChickenAPI.Enums.Game.Character;

namespace ChickenAPI.Packets.World.Server
{
    public class CListPacket : IPacket
    {
        public string Header => "clist";
        public Type Type => typeof(CListPacket);

        /// <summary>
        ///index 0
        /// </summary>
        public byte Slot { get; set; }

        /// <summary>
        ///index 1
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///index 2
        /// </summary>
        public byte Unknown { get; set; }

        /// <summary>
        ///index 3
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        ///index 4
        /// </summary>
        public HairStyleType HairStyle { get; set; }

        /// <summary>
        ///index 5
        /// </summary>
        public HairColorType HairColor { get; set; }

        /// <summary>
        ///index 6
        /// </summary>
        public byte Unknown1 { get; set; }

        /// <summary>
        ///index 7
        /// </summary>
        public CharacterClassType Class { get; set; }

        /// <summary>
        ///index 8
        /// </summary>
        public ushort Level { get; set; }

        /// <summary>
        ///index 9
        /// </summary>
        public ushort HeroLevel { get; set; }

        /// <summary>
        ///index 10 - SeparatorBeforeProperty = " "
        /// </summary>
        public List<short?> Equipments { get; set; }

        /// <summary>
        ///index 11
        /// </summary>
        public ushort JobLevel { get; set; }

        /// <summary>
        ///index 12
        /// </summary>
        public byte QuestCompletion { get; set; }

        /// <summary>
        ///index 13
        /// </summary>
        public byte QuestPart { get; set; }

        /// <summary>
        ///index 14
        /// </summary>
        public List<short?> Pets { get; set; }

        /// <summary>
        ///index 15
        /// </summary>
        public int Design { get; set; }

        /// <summary>
        ///index 16
        /// </summary>
        public byte Unknown3 { get; set; }

        public string Serialize()
        {
            var packetBuilder = new StringBuilder();
            packetBuilder.Append(Header);
            packetBuilder.AppendFormat(" {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} ", Slot, Name, Unknown, Gender, HairStyle, HairColor, Unknown1, Class, Level, HeroLevel );
            packetBuilder.Append(string.Join(" ", Equipments));
            packetBuilder.AppendFormat(" {0} {1} {2} ", JobLevel, QuestCompletion, QuestPart);
            packetBuilder.Append(string.Join(".", Pets));
            packetBuilder.AppendFormat(" {0} {1}", Design, Unknown3);

            return packetBuilder.ToString();
        }

        public void Deserialize(string[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}