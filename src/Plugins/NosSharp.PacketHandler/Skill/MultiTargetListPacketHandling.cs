using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.ClientPackets.Battle;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Skill
{
    public class MultiTargetListPacketHandling : GenericGamePacketHandlerAsync<MultiTargetListPacket>
    {
        public MultiTargetListPacketHandling(ILogger log) : base(log)
        {
        }

        protected override Task Handle(MultiTargetListPacket packet, IPlayerEntity player)
        {
            return Task.CompletedTask;
        }
    }
}