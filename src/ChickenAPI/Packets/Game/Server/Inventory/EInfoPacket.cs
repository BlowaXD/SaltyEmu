namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("e_info")]
    public class EInfoPacket : PacketBase
    {
        [PacketIndex(0)]
        public byte EInfoType { get; set; }
    }
}