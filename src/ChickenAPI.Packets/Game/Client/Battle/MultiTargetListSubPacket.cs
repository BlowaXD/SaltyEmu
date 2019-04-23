using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Battle
{
    [PacketHeader("multi_target_list_sub_packet")]
    public class MultiTargetListSubPacket : PacketBase
    {
        [PacketIndex(0)]
        public int SkillCastId { get; set; }

        [PacketIndex(1)]
        public int TargetId { get; set; }
    }
}