using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Game.Components;
using ChickenAPI.Game.Packets.Game.Client;
using ChickenAPI.Game.Packets.Game.Server;

namespace NosSharp.PacketHandler
{
    public class NcifPacketHandling
    {
        public static void OnNcifPacket(NcifPacket packetBase, IPlayerEntity player)
        {
            IEntity entity;
            switch (packetBase.Type)
            {
                case VisualType.Character:
                    break;
                case VisualType.Npc:
                    entity = player.EntityManager.GetEntitiesByType<IEntity>(EntityType.Npc).FirstOrDefault(s => s.GetComponent<NpcMonsterComponent>().MapNpcMonsterId == packetBase.TargetId);
                    if (entity == null)
                    {
                        return;
                    }
                    player.SendPacket(new StPacket(entity));
                    break;
                case VisualType.Monster:
                    entity = player.EntityManager.GetEntitiesByType<IEntity>(EntityType.Monster).FirstOrDefault(s => s.GetComponent<NpcMonsterComponent>().MapNpcMonsterId == packetBase.TargetId);
                    if (entity == null)
                    {
                        return;
                    }
                    player.SendPacket(new StPacket(entity));
                    break;
                case VisualType.MapObject:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}