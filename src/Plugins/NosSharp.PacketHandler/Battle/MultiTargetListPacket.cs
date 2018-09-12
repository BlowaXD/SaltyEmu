using System;
using System.Collections.Generic;
using System.Text;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client._NotYetSorted;

namespace NosSharp.PacketHandler.Battle
{
    public class MultiTargetListPacket
    {
        private static readonly Logger Log = Logger.GetLogger<MultiTargetListPacket>();

        public static void OnMultiTargetListPacket(MultiTargetListPacket packet, IPlayerEntity session)
        {

        }
    }
}
