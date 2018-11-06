using System;
using System.Collections.Generic;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Maps.Events;
using ChickenAPI.Game.Maps.Extensions;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.PacketHandling.Extensions;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Game.Visibility.Events;
using ChickenAPI.Packets.Game.Server.Group;

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
                    BroadcastInPacket(joinEvent);
                    if (!(joinEvent.Sender is IPlayerEntity player))
                    {
                        return;
                    }

                    SendCharacterInformationsPackets(player);
                    SendRightPackets(player);
                    SendMapInfosPackets(player, joinEvent);
                    break;
            }
        }

        private void SendMapInfosPackets(IPlayerEntity player, MapJoinEvent join)
        {
            player.SendPackets(join.Map.GetEntitiesPackets());
            // todo pairy
            player.SendPackets(join.Map.GetShopsPackets());
            player.SendPackets(join.Map.GetPortalsPackets());
        }

        private static void SendCharacterInformationsPackets(IPlayerEntity player)
        {
            player.SendPacket(player.GenerateCInfoPacket());
            player.SendPacket(player.GenerateCModePacket());
            player.SendPacket(player.GenerateEqPacket());
            player.SendPacket(player.GenerateEquipmentPacket());
            player.SendPacket(player.GenerateLevPacket());
            player.SendPacket(player.GenerateStPacket());
        }

        private static void SendRightPackets(IPlayerEntity player)
        {
            player.SendPacket(player.GenerateAtPacket());
            player.SendPacket(player.GenerateCondPacket());
            player.SendPacket(player.CurrentMap.Map.GenerateCMapPacket());
            player.SendPacket(player.GenerateStatCharPacket());
            player.SendPacket(player.GeneratePairyPacket());
            // Pst()
            // Act6() : Act()
            player.SendPacket(new PInitPacket());
            // PInitPacket
            // ScPacket
            // ScpStcPacket
            // FcPacket
            // Act4Raid ? DgPacket() : RaidMbf
            // MapDesignObjects()
            // MapDesignObjectsEffects
            // MapItems()
            // Gp()
            //SendPacket(new RsfpPacket()); // Minimap Position
            //SendPacket(new CondPacketBase(this));
            player.EmitEvent(new VisibilitySetVisibleEventArgs
            {
                Broadcast = true,
                IsChangingMapLayer = true
            });
            player.SendPacket(player.GenerateInPacket());
            player.SendPacket(player.GenerateStatPacket());
        }

        private static void BroadcastInPacket(MapJoinEvent joinEvent)
        {
            joinEvent.Map.Broadcast(joinEvent.Sender.GenerateInPacket());
        }
    }
}