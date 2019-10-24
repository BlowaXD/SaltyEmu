using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.ClientPackets.Battle;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Skill
{
    public class UseAoeSkillPacketHandler : GenericGamePacketHandlerAsync<UseAoeSkillPacket>
    {
        public UseAoeSkillPacketHandler(ILogger log) : base(log)
        {
        }

        protected override Task Handle(UseAoeSkillPacket packet, IPlayerEntity player) => Task.CompletedTask;
    }
}