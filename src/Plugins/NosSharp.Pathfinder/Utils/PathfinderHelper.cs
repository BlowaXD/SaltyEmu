using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Maps;
using System;
using System.Collections.Generic;
using System.Text;

namespace NosSharp.Pathfinder.Utils
{
    public static class PathfinderHelper
    {
        private static readonly sbyte[,] Neighbours =
        {
            { -1, -1 }, { 0, -1 }, { 1, -1 },
            { -1, 0 }, { -1, 1 },
            { -1, 1 }, { 0, 1 }, { 1, 1 }
        };

        public static List<Position<short>> GetNeighbors(Position<short> pos, IMap map)
        {
            List<Position<short>> neighbors = new List<Position<short>>();
            for (byte i = 0; i < Neighbours.Length; i++)
            {
                short x = (short)(pos.X + Neighbours[i, 0]),
                    y = (short)(pos.Y + Neighbours[i, 1]);

                if (x > 0 && x < map.Width && y > 0 && y < map.Height && map.IsWalkable(x, y))
                {
                    neighbors.Add(new Position<short> { X = x, Y = y });
                }
            }
            return neighbors;
        }

        public static int NextCoord(short start, short end) => start - end >= 1 ? start + 1 : end == start ? end : start - 1;
    }
}
