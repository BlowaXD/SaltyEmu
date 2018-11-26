using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.Shop;
using ChickenAPI.Game.Data.AccessLayer.Map;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps;

namespace SaltyEmu.BasicPlugin
{
    public class LazyMapManager : IMapManager
    {
        private readonly Dictionary<Guid, IMapLayer> _mapLayers = new Dictionary<Guid, IMapLayer>();
        private readonly Dictionary<long, IMap> _maps = new Dictionary<long, IMap>();

        public IReadOnlyDictionary<long, IMap> Maps { get; }

        public void ChangeMap(IPlayerEntity player, long mapId)
        {
            throw new NotImplementedException();
        }

        public void ChangeMapLayer(IPlayerEntity player, Guid mapLayerId)
        {
            throw new NotImplementedException();
        }

        public void ChangeMapLayer(IPlayerEntity player, IMapLayer layer)
        {
            throw new NotImplementedException();
        }

        public IMapLayer GetBaseMapLayer(long mapId)
        {
            if (_maps.TryGetValue(mapId, out IMap map))
            {
                return map.BaseLayer;
            }

            map = LoadMap(mapId);

            if (map is null)
            {
                return null;
            }

            _maps.Add(mapId, map);
            _mapLayers.Add(map.BaseLayer.Id, map.BaseLayer);

            return map.BaseLayer;
        }

        public IMapLayer GetBaseMapLayer(IMap map) => map.BaseLayer;

        private static IMap LoadMap(long mapId)
        {
            MapDto map = ChickenContainer.Instance.Resolve<IMapService>().GetById(mapId);

            if (map is null)
            {
                return null;
            }

            IEnumerable<MapNpcDto> npcs = ChickenContainer.Instance.Resolve<IMapNpcService>().GetByMapId(mapId);
            IEnumerable<MapMonsterDto> monsters = ChickenContainer.Instance.Resolve<IMapMonsterService>().GetByMapId(mapId);
            IEnumerable<PortalDto> portals = ChickenContainer.Instance.Resolve<IPortalService>().GetByMapId(mapId);
            IEnumerable<ShopDto> shops = ChickenContainer.Instance.Resolve<IShopService>().GetByMapNpcIds(npcs.Select(s => s.Id));

            return new SimpleMap(map, monsters, npcs, portals, shops);
        }
    }
}