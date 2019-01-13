using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Mates
{
    [PacketHeader("say_p")]
    public class SayPetPacket
    {
        [PacketIndex(0)]
        public long PetId { get; set; }

        [PacketIndex(1, SerializeToEnd = true)]
        public string Message { get; set; }
    }
}