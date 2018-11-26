using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.Shop;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.IAs;
using ChickenAPI.Game.Movements;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Maps
{
    public class SimpleMapLayer : EntityManagerBase, IMapLayer
    {
        public SimpleMapLayer(IMap map, IEnumerable<MapMonsterDto> monsters,
            IEnumerable<MapNpcDto> npcs = null,
            IEnumerable<PortalDto> portals = null,
            IEnumerable<ShopDto> shops = null,
            bool initSystems = true)
        {
            Id = Guid.NewGuid();
            Map = map;
            ParentEntityManager = map;
            InitializeMonsters(monsters);
            InitializeNpcs(npcs, shops);
            InitializePortals(portals);

            if (initSystems)
            {
                InitializeSystems();
            }
        }

        private void InitializePortals(IEnumerable<PortalDto> portals)
        {
            if (portals == null)
            {
                return;
            }

            foreach (PortalDto portal in portals)
            {
                new PortalEntity(portal).TransferEntity(this);
            }
        }

        private void InitializeSystems()
        {
            AddSystem(new MovableSystem(this));
            AddSystem(new IASystem(this, Map));
            AddSystem(new EffectSystem(this));
            AddSystem(new RespawnSystem(this));
        }

        private void InitializeNpcs(IEnumerable<MapNpcDto> npcs, IEnumerable<ShopDto> shops)
        {
            if (npcs == null)
            {
                return;
            }

            foreach (MapNpcDto npc in npcs)
            {
                ShopDto shop = shops?.FirstOrDefault(s => s.MapNpcId == npc.Id);
                new NpcEntity(npc, shop).TransferEntity(this);
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
                new MonsterEntity(monster).TransferEntity(this);
            }
        }

        private long _transportId = 10000;

        public Guid Id { get; set; }
        public IMap Map { get; }
        public IEnumerable<IPlayerEntity> Players => _players;

        public IPlayerEntity GetPlayerById(long id)
        {
            return GetEntity<IPlayerEntity>(id, VisualType.Character);
        }

        public IEnumerable<IPlayerEntity> GetPlayersInRange(Position<short> pos, int range) =>
            _players.Where(e => PositionHelper.GetDistance(pos, e.Movable.Actual) < range);

        public IEnumerable<IEntity> GetEntitiesInRange(Position<short> pos, int range) =>
            Entities.Where(e => e.HasComponent<MovableComponent>() && PositionHelper.GetDistance(pos, e.GetComponent<MovableComponent>().Actual) < range);

        public IEnumerable<T> GetEntitiesInRange<T>(Position<short> pos, int range) where T : IEntity =>
            Entities.Where(e => e is T && e.HasComponent<MovableComponent>() && PositionHelper.GetDistance(pos, e.GetComponent<MovableComponent>().Actual) < range) as IEnumerable<T>;

        public void Broadcast<T>(T packet) where T : IPacket => Broadcast(packet, null);


        public void Broadcast<T>(IEnumerable<T> packets) where T : IPacket => Broadcast(packets, null);

        public void Broadcast(IEnumerable<IPacket> packets) => Broadcast(packets, null);

        public void Broadcast<T>(T packet, IBroadcastRule rule) where T : IPacket
        {
            foreach (IPlayerEntity i in Players)
            {
                if (rule == null || rule.Match(i))
                {
                    i.SendPacket(packet);
                }
            }
        }

        public void Broadcast<T>(IEnumerable<T> packets, IBroadcastRule rule) where T : IPacket
        {
            foreach (IPlayerEntity i in Players)
            {
                if (rule == null || rule.Match(i))
                {
                    i.SendPackets(packets);
                }
            }
        }

        public void Broadcast(IEnumerable<IPacket> packets, IBroadcastRule rule)
        {
            foreach (IPlayerEntity i in Players)
            {
                if (rule == null || rule.Match(i))
                {
                    i.SendPackets(packets);
                }
            }
        }

        public long GetNextId() => _transportId++;
    }
}