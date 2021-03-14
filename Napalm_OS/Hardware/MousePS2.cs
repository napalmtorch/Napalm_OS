using System;
using System.Collections.Generic;
using System.Text;
using Napalm_OS.Core;
using Napalm_OS.Graphics;

namespace Napalm_OS.Hardware
{
    public static class MousePS2
    {
        // cursor data
        public static readonly uint[] CursorDefault = new uint[12 * 20]
        {
             Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Black.PackedValue, Color.Black.PackedValue, Color.Black.PackedValue, Color.Black.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Black.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Black.PackedValue, Color.White.PackedValue, Color.White.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Black.PackedValue, Color.Black.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
             Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue, Color.Magenta.PackedValue,
        };

        // position
        public static uint X { get { return Cosmos.System.MouseManager.X; } }
        public static uint Y { get { return Cosmos.System.MouseManager.Y; } }
        public static Cosmos.System.MouseState State { get { return Cosmos.System.MouseManager.MouseState; } }

        // initialization
        public static void Initialize()
        {
            Cosmos.System.MouseManager.ScreenWidth = Graphics2D.Width;
            Cosmos.System.MouseManager.ScreenHeight = Graphics2D.Height;
        }

        // draw
        public static void Display()
        {
            Graphics2D.DrawArray(X, Y, 12, 20, CursorDefault, Color.Magenta);
        }
    }
}
