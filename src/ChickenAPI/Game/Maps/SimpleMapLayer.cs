using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.Features.Shops;
using ChickenAPI.Game.Game.Components;
using ChickenAPI.Game.Game.Systems.Chat;
using ChickenAPI.Game.Game.Systems.Inventory;
using ChickenAPI.Game.Game.Systems.Movable;
using ChickenAPI.Game.Game.Systems.Visibility;
using ChickenAPI.Game.Packets;

namespace ChickenAPI.Game.Game.Maps
{
    public class SimpleMapLayer : EntityManagerBase, IMapLayer
    {
        public SimpleMapLayer(IMap map, IEnumerable<MapMonsterDto> monsters, IEnumerable<MapNpcDto> npcs = null, IEnumerable<PortalDto> portals = null, IEnumerable<ShopDto> shops = null)
        {
            Id = Guid.NewGuid();
            Map = map;
            ParentEntityManager = map;
            NotifiableSystems = new Dictionary<Type, INotifiableSystem>
            {
                { typeof(VisibilitySystem), new VisibilitySystem(this) },
                { typeof(ChatSystem), new ChatSystem(this) },
                { typeof(MovableSystem), new MovableSystem(this) },
                { typeof(InventorySystem), new InventorySystem(this) },
                { typeof(ShopSystem), new ShopSystem(this) }
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
                ShopDto shop = shops?.FirstOrDefault(s => s.MapNpcId == npc.Id);
                RegisterEntity(new NpcEntity(npc, shop));
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

        public IEnumerable<IEntity> GetEntitiesInRange(Position<short> pos, int range) =>
            Entities.Where(e => e.HasComponent<MovableComponent>() && PositionHelper.GetDistance(pos, e.GetComponent<MovableComponent>().Actual) < range);

        public void Broadcast<T>(T packet) where T : IPacket
        {
            foreach (IPlayerEntity i in GetEntitiesByType<IPlayerEntity>(EntityType.Player))
            {
                i.SendPacket(packet);
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
    }
}