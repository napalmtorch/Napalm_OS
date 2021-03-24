using System;
using System.Collections.Generic;
using System.Text;

namespace NapalmOS
{
    public struct Point
    {
        // properties
        public int X { get; set; }
        public int Y { get; set; }

        // constructor
        public Point(int x = 0, int y = 0) { this.X = x; this.Y = y; }

        // set
        public void Set(int x, int y) { this.X = x; this.Y = y; }
    }
}
