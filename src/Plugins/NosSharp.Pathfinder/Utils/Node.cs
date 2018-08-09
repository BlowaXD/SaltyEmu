using ChickenAPI.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace NosSharp.Pathfinder.Utils
{
    public class Node : IComparable
    {
        public double G { get; set; }

        public int H { get; set; }

        public double F { get; set; }

        public int Distance { get; set; }

        public bool Closed { get; set; }

        public Position<short> Position { get; set; }

        public Node Parent { get; set; }

        /// <summary>
        /// Compare the F properties with another Node
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj) => obj is Node node ? (F * 10).CompareTo(node.F * 10) : 0;
    }
}
