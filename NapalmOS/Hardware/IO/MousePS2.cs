using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.Graphics;

namespace NapalmOS.Hardware
{
    public static class MousePS2
    {
        // position
        public static int X { get { return (int)Cosmos.System.MouseManager.X; } }
        public static int Y { get { return (int)Cosmos.System.MouseManager.Y; } }
        public static Cosmos.System.MouseState State { get { return Cosmos.System.MouseManager.MouseState; } }

        // cursor
        public static Image ImgCursor;
        private static readonly byte[] CursorData = new byte[6 * 10]
        {
            0x00, 0x9F, 0x9F, 0x9F, 0x9F, 0x9F,
            0x00, 0x00, 0x9F, 0x9F, 0x9F, 0x9F,
            0x00, 0xFF, 0x00, 0x9F, 0x9F, 0x9F,
            0x00, 0xFF, 0xFF, 0x00, 0x9F, 0x9F,
            0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x9F,
            0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00,
            0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x9F,
            0x00, 0xFF, 0x00, 0xFF, 0x00, 0x9F,
            0x00, 0x00, 0x9F, 0x00, 0xFF, 0x00,
            0x9F, 0x9F, 0x9F, 0x9F, 0x00, 0x9F,
        };

        // task
        public static Task Task = new Task("MS Driver", "msdrv.sys");

        // initialization
        public static void Initialize(uint w, uint h)
        {
            // set screen reference size
            Cosmos.System.MouseManager.ScreenWidth = w;
            Cosmos.System.MouseManager.ScreenHeight = h;

            // load cursor
            ImgCursor = new Image(6, 10);
            ImgCursor.LoadData(6, 10, CursorData);

            // register task
            TaskManager.RegisterTask(Task);
        }

        // draw mouse
        public static void Display()
        {
            // draw
            Renderer.DrawImage(X, Y, Color.Magenta, ImgCursor);
        }
    }
}
