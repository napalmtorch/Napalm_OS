using System;
using System.Collections.Generic;
using System.Text;
using Napalm_OS.Hardware;

namespace Napalm_OS.Graphics
{
    // graphics drivers
    public enum GraphicsDriver : int
    {
        VMWareSVGA,
        VGA,
        VBE,
    }

    // fonts
    public enum FontType : int
    {
        Regular6x8,
        Regular9x16,
    }

    public static class Graphics2D
    {
        // mode
        public static uint Width { get; private set; } = 640;
        public static uint Height { get; private set; } = 480;
        public static uint Depth { get; private set; } = 32;
        public static GraphicsDriver Driver { get; private set; }

        // devices
        public static VMWareSVGA SVGADevice { get; private set; }

        // fonts
        private static uint FontWidth, FontHeight;
        public static readonly List<Font> Fonts = new List<Font>()
        {
            new Font6x8(),
            new Font9x16(),
        };

        // performance
        public static uint FPS { get; private set; }
        public static float Delta { get; private set; }
        private static uint time, lastTime, frames;

        // initialize
        public static void Initialize(GraphicsDriver driver)
        {
            // set driver
            Driver = driver;

            // vmware svga driver
            if (driver == GraphicsDriver.VMWareSVGA)
            { 
                if (SVGADevice == null) { SVGADevice = new VMWareSVGA(); }
                SVGADevice.SetMode(Width, Height);
            }
            // generic vga driver
            else if (driver == GraphicsDriver.VGA)
            {

            }
        }

        // clear screen
        public static void Clear(Color color)
        {
            if (Driver == GraphicsDriver.VMWareSVGA)
            {
                SVGADevice.DoubleBuffer_Clear(color.PackedValue);
            }
        }

        // push buffer to screen
        public static void Display()
        {
            if (Driver == GraphicsDriver.VMWareSVGA)
            {
                SVGADevice.DoubleBuffer_Update();
            }
        }

        // update framerate
        public static void UpdateFramerate()
        {
            frames++;
            time = Cosmos.HAL.RTC.Second;
            if (time != lastTime)
            {
                FPS = frames;
                frames = 0;
                lastTime = time;
            }
        }

        // draw pixel
        public static void DrawPixel(uint x, uint y, Color color)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) { return; }
            if (Driver == GraphicsDriver.VMWareSVGA)
            {
                SVGADevice.DoubleBuffer_SetPixel(x, y, color.PackedValue);
            }
        }

        // draw pixel using packed value
        public static void DrawPixel(uint x, uint y, uint color)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) { return; }
            if (Driver == GraphicsDriver.VMWareSVGA)
            {
                SVGADevice.DoubleBuffer_SetPixel(x, y, color);
            }
        }

        // draw filled rectangle
        public static void DrawFilledRect(uint x, uint y, uint w, uint h, Color color)
        {
            if (Driver == GraphicsDriver.VMWareSVGA)
            {
                SVGADevice.DoubleBuffer_DrawFillRectangle(x, y, w, h, color.PackedValue);
            }
        }

        // draw character with transparent background
        public static void DrawChar(uint x, uint y, char c, Color color, FontType font)
        {
            // get character data
            FontWidth = Fonts[(int)font].CharWidth; FontHeight = Fonts[(int)font].CharHeight;
            Graphics.FontCharacter fontChar = Font.CharToFontChar(c);

            // not pixel
            if (fontChar != FontCharacter.Pixel)
            {
                for (uint i = 0; i < FontWidth * FontHeight; i++)
                {
                    if ((int)fontChar < Fonts[(int)font].Characters.Count)
                    {
                        if (Fonts[(int)font].Characters[(int)fontChar][i] >= 1) { DrawPixel(x + (i % FontWidth), y + (i / FontWidth), color); }
                    }
                }
            }
        }

        // draw string of text with transparent background
        public static void DrawText(uint x, uint y, string text, Color color, FontType font)
        {
            uint dx = x, dy = y;
            FontWidth = Fonts[(int)font].CharWidth; FontHeight = Fonts[(int)font].CharHeight;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n') { dx = x; dy += FontHeight + Fonts[(int)font].CharSpacingY; }
                else
                {
                    DrawChar(dx, dy, text[i], color, font);
                    dx += FontWidth + Fonts[(int)font].CharSpacingX;
                }
            }
        }

        // draw array of colors
        public static void DrawArray(uint x, uint y, uint w, uint h, uint[] data, Color transparencyKey)
        {
            for (uint i = 0; i < w * h; i++)
            {
                if (data[i] != transparencyKey.PackedValue)
                {
                    DrawPixel(x + (i % w), y + (i / w), data[i]);
                }
            }
        }
    }
}
