using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Data.Shop;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game._ECS.Entities;

namespace SaltyEmu.BasicPlugin
{
    public class LazyMapManager : IMapManager
    {
        private readonly Dictionary<Guid, IMapLayer> _mapLayers = new Dictionary<Guid, IMapLayer>();
        private readonly Dictionary<long, IMap> _maps = new Dictionary<long, IMap>();
        private readonly IMapNpcService _npcService;
        private readonly IMapMonsterService _monsterService;
        private readonly INpcMonsterSkillService _npcMonsterSkillService;
        private readonly IPortalService _portalService;
        private readonly IShopService _shopService;
        private readonly IMapService _mapService;

        public LazyMapManager(IMapNpcService npcService, IMapMonsterService monsterService, IPortalService portalService, IShopService shopService, IMapService mapService, INpcMonsterSkillService npcMonsterSkillService)
        {
            _npcService = npcService;
            _monsterService = monsterService;
            _portalService = portalService;
            _shopService = shopService;
            _mapService = mapService;
            _npcMonsterSkillService = npcMonsterSkillService;
        }

        public IReadOnlyDictionary<long, IMap> Maps => _maps;


        public IMapLayer GetBaseMapLayer(long mapId)
        {
            if (_maps.TryGetValue(mapId, out IMap map))
            {
                return map.BaseLayer;
            }

            map = LoadMap(mapId).ConfigureAwait(false).GetAwaiter().GetResult();

            if (map is null)
            {
                return null;
            }

            _maps.Add(mapId, map);
            _mapLayers.Add(map.BaseLayer.Id, map.BaseLayer);

            return map.BaseLayer;
        }

        public IMapLayer GetBaseMapLayer(IMap map) => map.BaseLayer;

        private async Task<IMap> LoadMap(long mapId)
        {
            MapDto map = _mapService.GetById(mapId);

            if (map is null)
            {
                return null;
            }

            IEnumerable<MapNpcDto> npcs = await _npcService.GetByMapIdAsync(mapId);
            IEnumerable<MapMonsterDto> monsters = await _monsterService.GetByMapIdAsync(mapId);
            IEnumerable<PortalDto> portals = await _portalService.GetByMapIdAsync(mapId);
            IEnumerable<ShopDto> shops = await _shopService.GetByMapNpcIdsAsync(npcs.Select(s => s.Id));
            List<long> npcMonsterIds = npcs.Select(s => s.NpcMonsterId).Intersect(monsters.Select(s => s.NpcMonsterId)).ToList();
            IEnumerable<NpcMonsterSkillDto> skills = await _npcMonsterSkillService.GetByNpcMonsterIdsAsync(npcMonsterIds);
 
            return new SimpleMap(map, monsters, npcs, portals, shops, skills);
        }
    }
}