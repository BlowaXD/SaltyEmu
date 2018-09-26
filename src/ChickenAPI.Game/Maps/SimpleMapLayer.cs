using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.Features.Effects;
using ChickenAPI.Game.Features.IAs;
using ChickenAPI.Game.Features.Movement;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Maps
{
    public class SimpleMapLayer : EntityManagerBase, IMapLayer
    {
        public SimpleMapLayer(IMap map, IEnumerable<MapMonsterDto> monsters, IEnumerable<MapNpcDto> npcs = null, IEnumerable<PortalDto> portals = null, IEnumerable<ShopDto> shops = null)
        {
            Id = Guid.NewGuid();
            Map = map;
            ParentEntityManager = map;
            InitializeMonsters(monsters);
            InitializeNpcs(npcs, shops);
            InitializePortals(portals);
            InitializeSystems();
        }

        public Guid Id { get; set; }
        public IMap Map { get; }

        public IEnumerable<IEntity> GetEntitiesInRange(Position<short> pos, int range) =>
            Entities.Where(e => e.HasComponent<MovableComponent>() && PositionHelper.GetDistance(pos, e.GetComponent<MovableComponent>().Actual) < range);

        public void Broadcast<T>(T packet) where T : IPacket
        {
            foreach (IPlayerEntity i in GetEntitiesByType<IPlayerEntity>(EntityType.Player))
            {
                i.SendPacket(packet);
            }
        }

        public void Broadcast<T>(IEnumerable<T> packets) where T : IPacket
        {
            foreach (IPlayerEntity i in GetEntitiesByType<IPlayerEntity>(EntityType.Player))
            {
                i.SendPackets(packets);
            }
        }

        public void Broadcast(IEnumerable<IPacket> packets)
        {
            foreach (IPlayerEntity i in GetEntitiesByType<IPlayerEntity>(EntityType.Player))
            {
                i.SendPackets(packets);
            }
        }

        public void Broadcast<T>(IPlayerEntity sender, T packet) where T : IPacket
        {
            foreach (IPlayerEntity i in GetEntitiesByType<IPlayerEntity>(EntityType.Player))
            {
                if (i == sender)
                {
                    continue;
                }

                i.SendPacket(packet);
            }
        }

        public void Broadcast<T>(IPlayerEntity sender, IEnumerable<T> packets) where T : IPacket
        {
            foreach (IPlayerEntity i in GetEntitiesByType<IPlayerEntity>(EntityType.Player))
            {
                if (i == sender)
                {
                    continue;
                }

                i.SendPackets(packets);
            }
        }

        private void InitializePortals(IEnumerable<PortalDto> portals)
        {
            if (portals == null)
            {
            }

            foreach (PortalDto portal in portals)
            {
                TransferEntity(new PortalEntity(portal), this);
            }
        }

        private void InitializeSystems()
        {
            AddSystem(new MovableSystem(this));
            AddSystem(new IASystem(this, Map));
            AddSystem(new EffectSystem(this));
        }

        private void InitializeNpcs(IEnumerable<MapNpcDto> npcs, IEnumerable<ShopDto> shops)
        {
            if (npcs == null)
            {
                return;
            }

            foreach (MapNpcDto npc in npcs)
            {
                ShopDto shop = shops.FirstOrDefault(s => s.MapNpcId == npc.Id);
                TransferEntity(new NpcEntity(npc, shop), this);
            }
        }

        private void InitializeMonsters(IEnumerable<MapMonsterDto> monsters)
        {
            if (monsters == null)
            {
                return;
            }

            foreach (MapMonsterDto monster in monsters)
            {
                TransferEntity(new MonsterEntity(monster), this);
            }
        }
    }
}