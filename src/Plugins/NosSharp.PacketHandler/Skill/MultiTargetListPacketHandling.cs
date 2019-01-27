using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client.Battle;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Skill
{
    public class MultiTargetListPacketHandling : GenericGamePacketHandlerAsync<MultiTargetListPacket>
    {
        protected override Task Handle(MultiTargetListPacket packet, IPlayerEntity player)
        {
            Log.Debug(packet.OriginalContent);
            return Task.CompletedTask;
        }
    }
}