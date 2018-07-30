using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Data.TransferObjects.Map;
using ChickenAPI.ECS.Entities;
using ChickenAPI.ECS.Systems;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.Systems.Chat;
using ChickenAPI.Game.Systems.Inventory;
using ChickenAPI.Game.Systems.Movable;
using ChickenAPI.Game.Systems.Visibility;
using ChickenAPI.Utils;

namespace ChickenAPI.Game.Maps
{
    public class SimpleMapLayer : EntityManagerBase, IMapLayer
    {
        public SimpleMapLayer(IMap map, IEnumerable<MapMonsterDto> monsters, IEnumerable<MapNpcDto> npcs = null, IEnumerable<PortalDto> portals = null)
        {
            Id = Guid.NewGuid();
            Map = map;
            ParentEntityManager = map;
            NotifiableSystems = new Dictionary<Type, INotifiableSystem>
            {
                { typeof(VisibilitySystem), new VisibilitySystem(this) },
                { typeof(ChatSystem), new ChatSystem(this) },
                { typeof(MovableSystem), new MovableSystem(this) },
                { typeof(InventorySystem), new InventorySystem(this) }
            };
            foreach (MapMonsterDto monster in monsters)
            {
                RegisterEntity(new MonsterEntity(monster));
            }

            if (npcs == null)
            {
                return;
            }

            foreach (MapNpcDto npc in npcs)
            {
                RegisterEntity(new NpcEntity(npc));
            }

            if (portals == null)
            {
                return;
            }
            foreach (PortalDto portal in portals)
            {
                RegisterEntity(new PortalEntity(portal));
            }
        }

        public Guid Id { get; set; }
        public IMap Map { get; }
        public IEnumerable<IEntity> GetEntitiesByType(EntityType type) => Entities.Where(s => s.Type == type);

        public IEnumerable<IEntity> GetEntitiesInRange(Position<short> pos, int range) =>
            Entities.Where(e => e.HasComponent<MovableComponent>() && PositionHelper.GetDistance(pos, e.GetComponent<MovableComponent>().Actual) < range);
    }
}