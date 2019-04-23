using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Quicklist.Events;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Skill
{
    public class QuickListPacketHandling : GenericGamePacketHandlerAsync<QsetPacket>
    {
        protected override async Task Handle(QsetPacket packet, IPlayerEntity player)
        {
            switch (packet.Type)
            {
                case 0:
                case 1:

                    if (packet.Data1.HasValue && packet.Data2.HasValue)
                    {
                        await player.EmitEventAsync(new QuicklistAddElementEvent
                        {
                            Type = packet.Type,
                            Q1 = packet.Q1,
                            Q2 = packet.Q2,
                            Data1 = packet.Data1.Value,
                            Data2 = packet.Data2.Value
                        });
                    }
                    else
                    {
                        await player.EmitEventAsync(new QuicklistAddElementEvent
                        {
                            Type = packet.Type,
                            Q1 = packet.Q1,
                            Q2 = packet.Q2
                        });
                    }

                    break;
                case 2:
                    if (packet.Data1.HasValue && packet.Data2.HasValue)
                    {
                        await player.EmitEventAsync(new QuicklistSwapElementEvent
                        {
                            Q1 = packet.Q1,
                            Q2 = packet.Q2,
                            Data1 = packet.Data1.Value,
                            Data2 = packet.Data2.Value
                        });
                    }
                    else
                    {
                        await player.EmitEventAsync(new QuicklistSwapElementEvent
                        {
                            Q1 = packet.Q1,
                            Q2 = packet.Q2
                        });
                    }

                    break;
                case 3:
                    if (packet.Data1.HasValue && packet.Data2.HasValue)
                    {
                        await player.EmitEventAsync(new QuicklistRemoveElementEvent
                        {
                            Q1 = packet.Q1,
                            Q2 = packet.Q2,
                            Data1 = packet.Data1.Value,
                            Data2 = packet.Data2.Value
                        });
                    }
                    else
                    {
                        await player.EmitEventAsync(new QuicklistRemoveElementEvent
                        {
                            Q1 = packet.Q1,
                            Q2 = packet.Q2
                        });
                    }

                    break;
            }
        }
    }
}