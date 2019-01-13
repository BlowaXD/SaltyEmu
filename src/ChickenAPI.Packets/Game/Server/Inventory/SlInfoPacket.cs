using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("slinfo")]
    public class SlInfoPacket : PacketBase
    {
        [PacketIndex(0)]
        public InventoryType InventoryType { get; set; }

        [PacketIndex(1)]
        public long ItemId { get; set; }

        [PacketIndex(2)]
        public short ItemMorph { get; set; }

        [PacketIndex(3)]
        public byte SpLevel { get; set; }

        [PacketIndex(4)]
        public byte LevelJobMinimum { get; set; }

        [PacketIndex(5)]
        public byte ReputationMinimum { get; set; }

        [PacketIndex(6)]
        public int Unknown { get; set; }

        [PacketIndex(7)]
        public int Unknown2 { get; set; }

        [PacketIndex(8)]
        public int Unknown3 { get; set; }

        [PacketIndex(9)]
        public int Unknown4 { get; set; }

        [PacketIndex(10)]
        public int Unknown5 { get; set; }

        [PacketIndex(11)]
        public int Unknown6 { get; set; }

        [PacketIndex(12)]
        public int Unknown7 { get; set; }

        [PacketIndex(13)]
        public byte SpType { get; set; }

        [PacketIndex(14)]
        public short FireResistance { get; set; }

        [PacketIndex(15)]
        public short WaterResistance { get; set; }

        [PacketIndex(16)]
        public short LightResistance { get; set; }

        [PacketIndex(17)]
        public short DarkResistance { get; set; }

        [PacketIndex(18)]
        public long Xp { get; set; }

        [PacketIndex(19)]
        public short SPXpData { get; set; }

        [PacketIndex(20)]
        public string Skill { get; set; }

        [PacketIndex(21)]
        public int TransportId { get; set; }

        [PacketIndex(22)]
        public int SlHit { get; set; }

        [PacketIndex(23)]
        public int FreeSpPoints { get; set; }

        [PacketIndex(24)]
        public int SlDefence { get; set; }

        [PacketIndex(25)]
        public int SlElement { get; set; }

        [PacketIndex(26)]
        public int SlHp { get; set; }

        [PacketIndex(27)]
        public byte Upgrade { get; set; }

        [PacketIndex(28)]
        public int Unknown8 { get; set; }

        [PacketIndex(29)]
        public int Unknown9 { get; set; }

        [PacketIndex(30)]
        public byte IsSpDestroyed { get; set; }

        [PacketIndex(31)]
        public int ShellHit { get; set; }

        [PacketIndex(32)]
        public int ShellDefense { get; set; }

        [PacketIndex(33)]
        public int ShellElement { get; set; }

        [PacketIndex(34)]
        public int ShellHpMp { get; set; }

        [PacketIndex(35)]
        public short SpStoneUpgrade { get; set; }

        [PacketIndex(36)]
        public short AttackPoints { get; set; }

        [PacketIndex(37)]
        public short DefensePoints { get; set; }

        [PacketIndex(38)]
        public short ElementPoints { get; set; }

        [PacketIndex(39)]
        public short HpMpPoints { get; set; }

        [PacketIndex(40)]
        public short SpFireResistance { get; set; }

        [PacketIndex(41)]
        public short SpWaterResistance { get; set; }

        [PacketIndex(42)]
        public short SpLightResistance { get; set; }

        [PacketIndex(43)]
        public short SpDarkResistance { get; set; }
    }
}