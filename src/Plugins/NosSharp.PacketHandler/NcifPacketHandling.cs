using System;
using System.Linq;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client._NotYetSorted;

namespace NosSharp.PacketHandler
{
    public class NcifPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<NcifPacketHandling>();

        public static void OnNcifPacket(NcifPacket packetBase, IPlayerEntity player)
        {
            try
            {
                IEntity entity;
                switch (packetBase.Type)
                {
                    case VisualType.Character:
                        break;
                    case VisualType.Npc:
                        entity = player.CurrentMap.GetEntitiesByType<IEntity>(VisualType.Npc).FirstOrDefault(s => s.GetComponent<NpcMonsterComponent>().MapNpcMonsterId == packetBase.TargetId);
                        if (entity == null)
                        {
                            return;
                        }

                        player.SendPacket(entity.GenerateStPacket());
                        break;
                    case VisualType.Monster:
                        entity = player.CurrentMap.GetEntitiesByType<IEntity>(VisualType.Monster).FirstOrDefault(s => s.GetComponent<NpcMonsterComponent>().MapNpcMonsterId == packetBase.TargetId);
                        if (entity == null)
                        {
                            return;
                        }

                        player.SendPacket(entity.GenerateStPacket());
                        break;
                    default:
                        return;
                }
            }
            catch (Exception e)
            {
                Log.Error("[NCIF]", e);
            }
        }
    }
}