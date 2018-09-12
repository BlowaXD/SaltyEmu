using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client._NotYetSorted;

namespace NosSharp.PacketHandler.Battle
{
    public class UseSkillPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<UseSkillPacketHandling>();

        public static void OnUseSkillPacket(UseSkillPacket packet, IPlayerEntity session)
        {

        }
    }
}
