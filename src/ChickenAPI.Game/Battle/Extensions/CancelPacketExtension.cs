using System;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Packets.Game.Server.QuickList.Battle;
using NLog.Targets;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class CancelPacketExtension
    {
        public static CancelPacket GenerateEmptyCancelPacket(this IBattleEntity battle, CancelPacketType type) => GenerateTargetCancelPacket(null, type);

        public static CancelPacket GenerateTargetCancelPacket(this IBattleEntity battle, CancelPacketType type)
        {
            return new CancelPacket
            {
                TargetId = battle?.Id ?? 0,
                Type = type,
            };
        }
    }
}