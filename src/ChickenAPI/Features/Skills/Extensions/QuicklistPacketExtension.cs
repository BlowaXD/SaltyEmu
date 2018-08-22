using System.Collections;
using System.Collections.Generic;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Quicklist;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Features.Skills.Extensions
{
    public static class QuicklistPacketExtension
    {
        public static IEnumerable<QSlotPacket> GenerateQuicklistPacket(this IPlayerEntity player)
        {
            return null;
        }
    }
}