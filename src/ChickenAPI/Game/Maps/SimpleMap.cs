using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace ChickenAPI.Game.Game.Maps
{
    public class SimpleMap : EntityManagerBase, IMap
    {
        private readonly MapDto _map;
        private readonly IEnumerable<MapMonsterDto> _monsters;
        private readonly IEnumerable<MapNpcDto> _npcs;
        private IMapLayer _baseMapLayer;

        public SimpleMap(MapDto map, IEnumerable<MapMonsterDto> monsters, IEnumerable<MapNpcDto> npcs, IEnumerable<PortalDto> portals, IEnumerable<ShopDto> shops)
        {
            _map = map;
            _monsters = monsters;
            _npcs = npcs;
            Portals = new HashSet<PortalDto>(portals);
            Shops = new HashSet<ShopDto>(shops);
            _baseMapLayer = new SimpleMapLayer(this, _monsters, _npcs, Portals, Shops);
            Layers = new HashSet<IMapLayer>();
        }

        public HashSet<PortalDto> Portals { get; }
        public HashSet<ShopDto> Shops { get; }

        public long Id => _map.Id;
        public int MusicId => _map.Music;
        public IMapLayer BaseLayer => _baseMapLayer ?? (_baseMapLayer = new SimpleMapLayer(this, _monsters, _npcs, Portals));
        public HashSet<IMapLayer> Layers { get; }

        public short Width => _map.Width;
        public short Height => _map.Height;
        public byte[] Grid => _map.Grid;

        public bool IsWalkable(short x, short y)
        {
            try
            {
                byte gridCell = Grid[x + y * Width];
                return gridCell == 0 || gridCell == 2 || gridCell >= 16 && gridCell <= 19;
            }
            catch (Exception)
            {
                Log.Warn($"[IS_WALKABLE] {Id}: {x} {y}");
                return false;
            }
        }
    }
}