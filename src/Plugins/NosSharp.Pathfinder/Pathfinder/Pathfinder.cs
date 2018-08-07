using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Maps;
using NosSharp.Pathfinder.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace NosSharp.Pathfinder.Pathfinders
{
    public class Pathfinder : IPathfinder
    {
        /// <summary> 
        /// Search if the path can be reached without collision, if not launches the search for the Astar 
        /// </summary> 
        /// <param name="start"></param> 
        /// <param name="end"></param> 
        /// <param name="map"></param> 
        /// <returns></returns> 
        public Position<short>[] FindPath(Position<short> start, Position<short> end, IMap map)
        {
            Position<short>[] path = new Position<short>[(int)Math.Floor(Math.Sqrt(Math.Abs(start.X - end.X) ^ 2 + Math.Abs(start.Y - end.Y) ^ 2))];
            Position<short> node = start;

            short i = 0;
            while (node != end)
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

    }
}