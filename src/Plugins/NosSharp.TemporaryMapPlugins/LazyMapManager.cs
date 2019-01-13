using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.Shop;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps;

namespace SaltyEmu.BasicPlugin
{
    public class LazyMapManager : IMapManager
    {
        private readonly Dictionary<Guid, IMapLayer> _mapLayers = new Dictionary<Guid, IMapLayer>();
        private readonly Dictionary<long, IMap> _maps = new Dictionary<long, IMap>();
        private readonly IMapNpcService _npcService;
        private readonly IMapMonsterService _monsterService;
        private readonly IPortalService _portalService;
        private readonly IShopService _shopService;
        private readonly IMapService _mapService;

        public LazyMapManager(IMapNpcService npcService, IMapMonsterService monsterService, IPortalService portalService, IShopService shopService, IMapService mapService)
        {
            _npcService = npcService;
            _monsterService = monsterService;
            _portalService = portalService;
            _shopService = shopService;
            _mapService = mapService;
        }

        public IReadOnlyDictionary<long, IMap> Maps => _maps;


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

        private IMap LoadMap(long mapId)
        {
            MapDto map = _mapService.GetById(mapId);

            if (map is null)
            {
                return null;
            }

            IEnumerable<MapNpcDto> npcs = _npcService.GetByMapId(mapId);
            IEnumerable<MapMonsterDto> monsters = _monsterService.GetByMapId(mapId);
            IEnumerable<PortalDto> portals = _portalService.GetByMapId(mapId);
            IEnumerable<ShopDto> shops = _shopService.GetByMapNpcIds(npcs.Select(s => s.Id));

            return new SimpleMap(map, monsters, npcs, portals, shops);
        }
    }
}