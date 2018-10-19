using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.Shop;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Maps
{
    public class SimpleMap : EntityManagerBase, IMap
    {
        private readonly MapDto _map;
        private readonly IEnumerable<MapMonsterDto> _monsters;
        private readonly IEnumerable<MapNpcDto> _npcs;
        private readonly IEnumerable<PortalDto> _portals;
        private readonly bool _initSystems;

        private readonly IRandomGenerator _random;
        private readonly IEnumerable<ShopDto> _shops;

        private readonly Position<short>[] _walkableGrid;
        private IMapLayer _baseMapLayer;

        public SimpleMap(MapDto map, IEnumerable<MapMonsterDto> monsters, IEnumerable<MapNpcDto> npcs, IEnumerable<PortalDto> portals, IEnumerable<ShopDto> shops, bool initSystems = true)
        {
            _map = map;
            _monsters = monsters;
            _npcs = npcs;
            _portals = portals;
            _shops = shops;
            _initSystems = initSystems;
            Layers = new HashSet<IMapLayer>();

            _random = ChickenContainer.Instance.Resolve<IRandomGenerator>();


            _walkableGrid = new Lazy<Position<short>[]>(() =>
            {
                List<Position<short>> cells = new List<Position<short>>();
                for (short y = 0; y <= map.Height; y++)
                {
                    for (short x = 0; x <= map.Width; x++)
                    {
                        cells.Add(new Position<short> { X = x, Y = y });
                    }
                }

                return cells.ToArray();
            }, LazyThreadSafetyMode.ExecutionAndPublication).Value;
        }

        public long Id => _map.Id;
        public int MusicId => _map.Music;
        public IMapLayer BaseLayer => _baseMapLayer ?? (_baseMapLayer = new SimpleMapLayer(this, _monsters, _npcs, _portals, _shops, _initSystems));
        public HashSet<IMapLayer> Layers { get; }

        public short Width => _map.Width;
        public short Height => _map.Height;
        public byte[] Grid => _map.Grid;

        public bool IsWalkable(short x, short y)
        {
            try
            {
                byte gridCell = Grid[x + y * Width];
                return IsWalkable(gridCell);
            }
            catch (Exception)
            {
                Log.Warn($"[IS_WALKABLE] {Id}: ({x},{y})");
                return false;
            }
        }

        public Position<short> GetFreePosition(short minimumX, short minimumY, short rangeX, short rangeY)
        {
            short minX = (short)(-rangeX + minimumX);
            short maxX = (short)(rangeX + minimumX);

            short minY = (short)(-rangeY + minimumY);
            short maxY = (short)(rangeY + minimumY);
            return _walkableGrid.Where(s => s.Y >= minY && s.Y <= maxY && s.X >= minX && s.X <= maxX).OrderBy(s => _random.Next(int.MaxValue)).FirstOrDefault(cell => IsWalkable(cell.X, cell.Y));
        }

        private static bool IsWalkable(byte cell) => cell == 0 || cell == 2 || cell >= 16 && cell <= 19;

        public PortalDto GetPortalFromPosition(short x, short y, short range = 2)
        {
            return _portals.FirstOrDefault(portal =>
                (x <= portal.SourceX + range && x >= portal.SourceX - range) &&
                (y <= portal.SourceY + range && y >= portal.SourceY - range));
        }
    }
}