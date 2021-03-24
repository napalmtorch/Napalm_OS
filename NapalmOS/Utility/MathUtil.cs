using System;
using System.Collections.Generic;
using System.Text;

namespace NapalmOS
{
    public static class MathUtil
    {
        // check if position is inside rectangle
        public static bool RectangleContains(int rx, int ry, int rw, int rh, int x, int y) { return (x >= rx && x < rx + rw && y >= ry && y < ry + rh); }
        public static bool RectangleContains(Rectangle rect, int x, int y) { return RectangleContains(rect.X, rect.Y, rect.Width, rect.Height, x, y); }
        public static bool RectangleContains(Rectangle rect, Point pos) { return RectangleContains(rect.X, rect.Y, rect.Width, rect.Height, pos.X, pos.Y); }

    }
}
