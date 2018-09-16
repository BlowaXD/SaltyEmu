using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Movement;
using ChickenAPI.Game.Packets;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Packets.Game.Client.Movement;

namespace NosSharp.PacketHandler.Move
{
    public class SitPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<SitPacketHandling>();

        public static void OnSitPacket(SitPacket packet, IPlayerEntity session)
        {
            packet?.Users.ForEach(u =>
            {
                if (u.UserType == 1)
                {
                    session.Movable.IsSitting = !session.Movable.IsSitting;
                    return;
                }
                //TODO: rest on mate
            });
        }
    }
}