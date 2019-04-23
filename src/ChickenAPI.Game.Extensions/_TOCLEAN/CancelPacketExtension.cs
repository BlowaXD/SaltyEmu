using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Battle;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class CancelPacketExtension
    {
        public static CancelPacket GenerateEmptyCancelPacket(this IBattleEntity battle, CancelPacketType type) => GenerateTargetCancelPacket(null, type);

        public static CancelPacket GenerateTargetCancelPacket(this IBattleEntity battle, CancelPacketType type) =>
            new CancelPacket
            {
                TargetId = battle?.Id ?? 0,
                Type = type
            };
    }
}