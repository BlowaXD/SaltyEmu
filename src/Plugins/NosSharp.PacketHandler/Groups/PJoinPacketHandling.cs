using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Groups.Args;
using ChickenAPI.Game.Managers;
using ChickenAPI.Packets.Game.Client.Movement;
using ChickenAPI.Packets.Game.Server.Group;
using NosSharp.PacketHandler.Move;

namespace NosSharp.PacketHandler.Groups
{
    public class PJoinPacketHandling
    {
        private static readonly IPlayerManager Manager = new Lazy<IPlayerManager>(() => ChickenContainer.Instance.Resolve<IPlayerManager>()).Value;
        private static readonly Logger Log = Logger.GetLogger<PreqPacketHandling>();

        public static void OnPreqPacket(PJoinPacket packet, IPlayerEntity player)
        {
            switch (packet.RequestType)
            {
                case PJoinPacketType.Requested:
                case PJoinPacketType.Invited:
                    player.EmitEvent(new GroupInvitationSendEvent
                    {
                        Target = player.CurrentMap.GetPlayerById(packet.CharacterId)
                    });
                    break;
                case PJoinPacketType.Accepted:
                    break;
                case PJoinPacketType.Declined:
                    break;
                case PJoinPacketType.Sharing:
                    break;
                case PJoinPacketType.AcceptedShare:
                    break;
                case PJoinPacketType.DeclinedShare:
                    break;
                default:
                    break;
            }
        }
    }
}