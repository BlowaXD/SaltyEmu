using System;
using System.Collections.Generic;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.Groups;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Maps.Events;
using ChickenAPI.Game.Maps.Extensions;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.PacketHandling.Extensions;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Game.Visibility.Events;
using ChickenAPI.Packets.Game.Server.Group;
using ChickenAPI.Packets.Game.Server.Map;

namespace ChickenAPI.Game.Maps
{
    public class MapEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(MapJoinEvent),
            typeof(MapLeaveEvent),
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case MapJoinEvent joinEvent:
                    if (!(joinEvent.Sender is IPlayerEntity player))
                    {
                        BroadcastInPacket(joinEvent);
                        return;
                    }

                    SendCharacterInformationsPackets(player);
                    SendRightPackets(player);
                    SendGroupPackets(player);
                    SendMapInfosPackets(player, joinEvent);
                    break;
                case MapLeaveEvent mapLeave:
                    if (!(mapLeave.Sender is IPlayerEntity session))
                    {
                        return;
                    }

                    session.SendPacket(new MapoutPacket());
                    session.BroadcastExceptSender(session.GenerateOutPacket());
                    break;
            }
        }

        private void SendMapInfosPackets(IPlayerEntity player, MapJoinEvent join)
        {
            // map design objects (mltobj)
            player.SendPackets(join.Map.GetPortalsPackets());
            // GenerateWp()
            player.SendPackets(join.Map.GetEntitiesPackets());
            player.SendPackets(join.Map.GetPairyPackets(player));
            player.SendPackets(join.Map.GetShopsPackets());

            // user shop
            // usershop (pflag) minimap
        }

        private static void SendCharacterInformationsPackets(IPlayerEntity player)
        {
            player.SendPacket(player.GenerateCInfoPacket());
            player.SendPacket(player.GenerateCModePacket());
            player.SendPacket(player.GenerateEqPacket());
            player.SendPacket(player.GenerateEquipmentPacket());
            player.SendPacket(player.GenerateLevPacket());
            player.SendPacket(player.GenerateStatPacket());
        }

        private static void SendRightPackets(IPlayerEntity player)
        {
            player.SendPacket(player.GenerateAtPacket());
            player.SendPacket(player.GenerateCondPacket());
            player.SendPacket(player.GenerateCMapPacket());
            player.SendPacket(player.GenerateStatCharPacket());
            player.SendPacket(player.GeneratePairyPacket());
        }

        private static void SendGroupPackets(IPlayerEntity player)
        {
            //player.SendPackets(player.GeneratePstPacket());
            // Act6() : Act()
            player.SendPacket(new PInitPacket());
            // ScPacket
            // ScpStcPacket
            // FcPacket
            // Act4Raid ? DgPacket() : RaidMbf
            // MapDesignObjects()
            // MapDesignObjectsEffects
            // MapItems()
            // Gp()
            //SendPacket(new RsfpPacket()); // Minimap Position
            player.SendPacket(player.GenerateCondPacket());
            player.Broadcast(player.GenerateInPacket());
            player.SendPacket(player.GenerateStatPacket());
        }

        private static void BroadcastInPacket(MapJoinEvent joinEvent)
        {
            joinEvent.Map.Broadcast(joinEvent.Sender.GenerateInPacket());
        }
    }
}