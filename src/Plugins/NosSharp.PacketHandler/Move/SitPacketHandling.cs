using System.Linq;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Movement;
using ChickenAPI.Game.Features.Movement.Args;
using ChickenAPI.Packets.Game.Client.Movement;
using NosSharp.PacketHandler.Utils;

namespace NosSharp.PacketHandler.Move
{
    public class SitPacketHandling : BasePacketHandling<SitPacket>
    {
        public override void OnPacketReceived(SitPacket packet, IPlayerEntity player)
        {
            foreach (SitSubPacket u in packet.Users)
            {
                if (u.UserType == 1 && u.UserId != player.Character.Id)
                {
                    return;
                }

                //TODO: rest on mate
            }

            player.NotifyEventHandler<MovementEventHandler>(new TriggerSitEvent
            {
                ChildsId = packet.Users.Select(s => s.UserId)
            });
        }
    }
}