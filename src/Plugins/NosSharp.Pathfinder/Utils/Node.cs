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

        public bool Opened { get; set; }

        public Node Parent { get; set; }

        public int CompareTo(object obj)
        {
            return (F*10).CompareTo((obj as Node).F*10);
        }
    }
}
