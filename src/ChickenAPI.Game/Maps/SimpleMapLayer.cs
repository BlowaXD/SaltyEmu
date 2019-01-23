using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Data.Shop;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.Groups;
using ChickenAPI.Game.IAs;
using ChickenAPI.Game.Movements;
using ChickenAPI.Game.Specialists;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Maps
{
    public class SimpleMapLayer : EntityManagerBase, IMapLayer
    {
        private static readonly IGameEntityFactory GameEntityFactory = new Lazy<IGameEntityFactory>(ChickenContainer.Instance.Resolve<IGameEntityFactory>).Value;

        private long _transportId = 10000000;

        public SimpleMapLayer(IMap map, IEnumerable<MapMonsterDto> monsters,
            IEnumerable<MapNpcDto> npcs = null,
            IEnumerable<PortalDto> portals = null,
            IEnumerable<ShopDto> shops = null,
            IEnumerable<NpcMonsterSkillDto> npcMonsterSkills = null,
            bool initSystems = true)
        {
            Id = Guid.NewGuid();
            Map = map;
            ParentEntityManager = map;
            Dictionary<long, List<NpcMonsterSkillDto>> tmpSkills = npcMonsterSkills.GroupBy(s => s.NpcMonsterId).ToDictionary(s => s.Key, s => s.ToList());
            InitializeMonsters(monsters, tmpSkills);
            InitializeNpcs(npcs, shops, tmpSkills);
            InitializePortals(portals);

            if (initSystems)
            {
                InitializeSystems();
            }
        }

        public Guid Id { get; set; }
        public IMap Map { get; }
        public bool IsPvpEnabled => false;
        public IEnumerable<IPlayerEntity> Players => _players;

        public IPlayerEntity GetPlayerById(long id) => GetEntity<IPlayerEntity>(id, VisualType.Character);

        public IEnumerable<IPlayerEntity> GetPlayersInRange(Position<short> pos, int range) =>
            _players.Where(e => PositionHelper.GetDistance(pos, e.Position) < range);

        public IEnumerable<IEntity> GetEntitiesInRange(Position<short> pos, int range) =>
            Entities.Where(e => e is IMovableEntity mov && PositionHelper.GetDistance(pos, mov.Position) < range);

        public IEnumerable<T> GetEntitiesInRange<T>(Position<short> pos, int range) where T : IEntity =>
            Entities.Where(e => e is T && e is IMovableEntity mov && PositionHelper.GetDistance(pos, mov.Position) < range) as IEnumerable<T>;

        public Task BroadcastAsync<T>(T packet) where T : IPacket => BroadcastAsync(packet, null);

        public Task BroadcastAsync<T>(T packet, IBroadcastRule rule) where T : IPacket
        {
            lock(LockObj)
            {
                return Task.WhenAll(_players.Select(s => Task.Run(() => rule == null || rule.Match(s) ? s.SendPacketAsync(packet) : Task.CompletedTask)));
            }
        }

        public Task BroadcastAsync<T>(IEnumerable<T> packets) where T : IPacket => BroadcastAsync(packets, null);

        public Task BroadcastAsync<T>(IEnumerable<T> packets, IBroadcastRule rule) where T : IPacket
        {
            lock(LockObj)
            {
                return Task.WhenAll(_players.Select(s => Task.Run(() => rule == null || rule.Match(s) ? s.SendPacketsAsync(packets) : Task.CompletedTask)));
            }
        }

        public Task BroadcastAsync(IEnumerable<IPacket> packets) => BroadcastAsync(packets, null);

        public Task BroadcastAsync(IEnumerable<IPacket> packets, IBroadcastRule rule)
        {
            lock(LockObj)
            {
                return Task.WhenAll(_players.Select(s => Task.Run(() => rule == null || rule.Match(s) ? s.SendPacketsAsync(packets) : Task.CompletedTask)));
            }
        }

        public long GetNextId() => _transportId++;

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
            AddSystem(new GroupSystem(this));
            AddSystem(new SpecialistSystem(this));
            AddSystem(new RespawnSystem(this));
        }

        private void InitializeNpcs(IEnumerable<MapNpcDto> npcs, IEnumerable<ShopDto> shops, IReadOnlyDictionary<long, List<NpcMonsterSkillDto>> skills)
        {
            if (npcs == null)
            {
                return;
            }

            foreach (MapNpcDto npc in npcs)
            {
                ShopDto shop = shops?.FirstOrDefault(s => s.MapNpcId == npc.Id);

                if (!skills.TryGetValue(npc.NpcMonsterId, out List<NpcMonsterSkillDto> npcSkills))
                {
                    npcSkills = new List<NpcMonsterSkillDto>();
                }
                new NpcEntity(npc, shop).TransferEntity(this);
            }
        }

        private void InitializeMonsters(IEnumerable<MapMonsterDto> monsters, IReadOnlyDictionary<long, List<NpcMonsterSkillDto>> skills)
        {
            if (monsters == null)
            {
                return;
            }

            foreach (MapMonsterDto monster in monsters)
            {
                if (!skills.TryGetValue(monster.NpcMonsterId, out List<NpcMonsterSkillDto> npcSkills))
                {
                    npcSkills = new List<NpcMonsterSkillDto>();
                }

                new MonsterEntity(monster, npcSkills).TransferEntity(this);
            }
        }
    }
}