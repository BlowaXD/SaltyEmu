using System;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.ECS.Entities;
using SaltyEmu.PathfinderPlugin.Utils;

namespace SaltyEmu.PathfinderPlugin.Algorithms
{
    public static class AStar
    {
        /// <summary>
        ///     Find a path using the AStar algorithm
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Position<short>[] FindPath(Position<short> start, Position<short> end, IMap map)
        {
            MiniHeap<Node> delNodes = new MiniHeap<Node>();
            MiniHeap<Node> nodes = new MiniHeap<Node>();
            nodes.Add(new Node { Position = start }, false);
            while (nodes.Count() > 0)
            {
                Node node = nodes.Pop();
                if (node.Position.Equals(end))
                {
                    int i = node.Distance;
                    Position<short>[] path = new Position<short>[i];
                    while (i >= 1)
                    {
                        path[--i] = node.Position;
                        node = node.Parent;
                    }

                    return path;
                }

                node.Closed = true;
                foreach (Position<short> n in PathfinderHelper.GetNeighbors(node.Position, map))
                {
                    if (n == null)
                    {
                        break;
                    }

                    Node neighbor = nodes.Get(no => no.Position.Equals(n)) ?? delNodes.Get(no => no.Position.Equals(n));
                    if (neighbor == null)
                    {
                        neighbor = new Node
                        {
                            Position = n,
                            G = node.G + (n.X == node.Position.X || n.Y == node.Position.Y ? 1 : 1.4),
                            H = Heuristics.Manhattan(Math.Abs(n.X - end.Y), Math.Abs(n.Y - end.Y)),
                            Parent = node,
                            Distance = node.Distance + 1
                        };
                        neighbor.F = neighbor.G + neighbor.H;
                        nodes.Add(neighbor);
                    }
                    else if (!neighbor.Closed)
                    {
                        double gScore = node.G + (n.X == node.Position.X || n.Y == node.Position.Y ? 1 : 1.4);
                        if (gScore > neighbor.G)
                        {
                            continue;
                        }

                        neighbor.G = gScore;
                        neighbor.F = neighbor.G + neighbor.H;
                        neighbor.Parent = node;
                        neighbor.Distance = node.Distance + 1;
                    }
                }

                delNodes.Add(node, false);
            }

            return null;
        }
    }
}