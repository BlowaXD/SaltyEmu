using System;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client.Battle;
using NosSharp.PacketHandler.Utils;

namespace NosSharp.PacketHandler.Battle
{
    public class MultiTargetListPacketHandling : BasePacketHandling<MultiTargetListPacket>
    {
        public override void OnPacketReceived(MultiTargetListPacket packet, IPlayerEntity player)
        {
            Log.Debug(packet.OriginalContent);
        }
    }
}