using System;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Maps;
using SaltyEmu.PathfinderPlugin.Utils;

namespace SaltyEmu.PathfinderPlugin.Algorithms
{
    public class Pathfinder : IPathfinder
    {
        /// <summary>
        ///     Search if the path can be reached without collision, if not launches the search for the Astar
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public Position<short>[] FindPath(Position<short> start, Position<short> end, IMap map)
        {
            Position<short>[] path = new Position<short>[Math.Max(Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y))];
            Position<short> node = start;

            short i = 0;
            while (!node.Equals(end))
            {
                short x = (short)PathfinderHelper.NextCoord(node.X, end.X),
                    y = (short)PathfinderHelper.NextCoord(node.Y, end.Y);
                if (!map.IsWalkable(x, y))
                {
                    return AStar.FindPath(start, end, map);
                }

                node = new Position<short> { X = x, Y = y };
                path[i++] = node;
            }

            return path;
        }


        /// <summary>
        ///     Returns an Array with the neighbors of the given position
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public Position<short>[] GetNeighbors(Position<short> pos, IMap map) => PathfinderHelper.GetNeighbors(pos, map);
    }
}