using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Movement.Extensions;
using ChickenAPI.Game.Packets;
using ChickenAPI.Game.Packets.Extensions;

namespace NosSharp.PacketHandler.Battle
{
    public class UseAoeSkillPacket
    {
        private static readonly Logger Log = Logger.GetLogger<UseAoeSkillPacket>();

        public static void OnUseAoeSkillPacket(UseAoeSkillPacket packet, IPlayerEntity session)
        {

        }
    }
}
