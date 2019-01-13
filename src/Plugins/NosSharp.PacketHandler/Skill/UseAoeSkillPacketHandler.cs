using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client.Battle;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Skill
{
    public class UseAoeSkillPacketHandler : GenericGamePacketHandlerAsync<UseAoeSkillPacket>
    {
        protected override Task Handle(UseAoeSkillPacket packet, IPlayerEntity player) => Task.CompletedTask;
    }
}