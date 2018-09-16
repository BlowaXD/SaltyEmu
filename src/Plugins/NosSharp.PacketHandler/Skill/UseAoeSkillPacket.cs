using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;

namespace NosSharp.PacketHandler.Skill
{
    public class UseAoeSkillPacket
    {
        private static readonly Logger Log = Logger.GetLogger<UseAoeSkillPacket>();

        public static void OnUseAoeSkillPacket(UseAoeSkillPacket packet, IPlayerEntity session)
        {

        }
    }
}
