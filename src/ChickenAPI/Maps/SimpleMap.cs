﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace ChickenAPI.Game.Maps
{
    public class SimpleMap : EntityManagerBase, IMap
    {
        private readonly MapDto _map;
        private readonly IEnumerable<MapMonsterDto> _monsters;
        private readonly IEnumerable<MapNpcDto> _npcs;
        private readonly IEnumerable<PortalDto> _portals;
        private readonly IEnumerable<ShopDto> _shops;

        private readonly Position<short>[] WalkableGrid;
        private IMapLayer _baseMapLayer;

        public SimpleMap(MapDto map, IEnumerable<MapMonsterDto> monsters, IEnumerable<MapNpcDto> npcs, IEnumerable<PortalDto> portals, IEnumerable<ShopDto> shops)
        {
            _map = map;
            _monsters = monsters;
            _npcs = npcs;
            _portals = portals;
            _shops = shops;
            _baseMapLayer = new SimpleMapLayer(this, _monsters, _npcs, _portals, _shops);
            Layers = new HashSet<IMapLayer>();


            List<Position<short>> cells = new List<Position<short>>();
            for (short y = 0; y <= _map.Height; y++)
            {
                for (short x = 0; x <= _map.Width; x++)
                {
                    cells.Add(new Position<short> { X = x, Y = y });
                }
            }

            WalkableGrid = cells.ToArray();
        }

        public long Id => _map.Id;
        public int MusicId => _map.Music;
        public IMapLayer BaseLayer => _baseMapLayer ?? (_baseMapLayer = new SimpleMapLayer(this, _monsters, _npcs, _portals, _shops));
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
                Log.Warn($"[IS_WALKABLE] {Id}: {x} {y}");
                return false;
            }
        }

        public Position<short> GetFreePosition(short minimumX, short minimumY, short rangeX, short rangeY)
        {
            var random = new Random();
            short minX = (short)(-rangeX + minimumX);
            short maxX = (short)(rangeX + minimumX);

            short minY = (short)(-rangeY + minimumY);
            short maxY = (short)(rangeY + minimumY);
            return WalkableGrid.Where(s => s.Y >= minY && s.Y <= maxY && s.X >= minX && s.X <= maxX).OrderBy(s => random.Next(int.MaxValue)).FirstOrDefault(cell => IsWalkable(cell.X, cell.Y));
        }

        private static bool IsWalkable(byte cell) => cell == 0 || cell == 2 || cell >= 16 && cell <= 19;
    }
}