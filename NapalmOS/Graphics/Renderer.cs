using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.GUI;
using NapalmOS.Hardware;

namespace NapalmOS.Graphics
{
    public static class Renderer
    {
        // performance information
        public static int FPS { get; private set; } = 0;
        public static double Delta { get; private set; } = 0;
        private static int frames, time, timeOld;

        // update performance info
        public static void UpdatePerformanceInfo()
        {
            frames++;
            time = Cosmos.HAL.RTC.Second;
            if (time != timeOld)
            {
                FPS = frames;
                Delta = (double)1000 / (double)frames;
                frames = 0;
                timeOld = time;
            }
        }

        // draw performance info
        public static void DrawPerformanceInfo(int x, int y, Color color, Font font)
        {
            int xx = x, yy = y;

            // fps
            DrawString(xx, yy, "FPS: " + FPS.ToString(), color, font);
            yy += font.Height + font.SpacingY;

            // delta
            string delta = Delta.ToString();
            if (delta.Length > 6) { delta = delta.Substring(0, 6); }
            DrawString(xx, yy, "DELTA: " + delta + " MS", color, font);
            yy += font.Height + font.SpacingY;

            // active
            DrawString(xx, yy, "ACTIVE: " + WindowManager.ActiveIndex.ToString(), color, font);
        }

        // clear the screen
        public static void Clear(Color color) { VGADriver.Clear(color); }

        // swap buffer
        public static void Swap(bool copy) { if (copy) { VGADriver.Display(); } else { VGADriver.DisplayAlt(); } }

        // draw pixel to window bounds
        public static bool DrawPixel(int x, int y, Color color)
        {
            // check if pixel is within client bounds
            if (!MathUtil.RectangleContains(0, 0, VGADriver.Mode.Width, VGADriver.Mode.Height, x, y)) { return false; }
            // draw pixel
            VGADriver.DrawPixel(x, y, color); return true;
        }

        // draw filled rectangle
        public static void DrawFilledRect(int x, int y, int w, int h, Color color)
        {
            for (int i = 0; i < w * h; i++) { DrawPixel(x + (i % w), y + (i / w), color); }
        }
        public static void DrawFilledRect(Rectangle rect, Color color) { DrawFilledRect(rect.X, rect.Y, rect.Width, rect.Height, color); }
        public static void DrawFilledRect(Point pos, Point size, Color color) { DrawFilledRect(pos.X, pos.Y, size.X, size.Y, color); }

        // draw rectangle outline
        public static void DrawRect(int x, int y, int w, int h, int thick, Color color)
        {
            DrawFilledRect(x, y, w, thick, color);
            DrawFilledRect(x, y, thick, h, color);
            DrawFilledRect(x + w - thick, y, thick, h, color);
            DrawFilledRect(x, y + h - thick, w, thick, color);
        }
        public static void DrawRect(Rectangle rect, int thick, Color color) { DrawRect(rect.X, rect.Y, rect.Width, rect.Height, thick, color); }
        public static void DrawRect(Point pos, Point size, int thick, Color color) { DrawRect(pos.X, pos.Y, size.X, size.Y, thick, color); }

        // draw 3d bordered rectangle
        public static void DrawRect3D(int x, int y, int w, int h, Color tl, Color br, bool invert)
        {
            if (invert)
            {
                DrawFilledRect(x, y, w - 1, 1, br);
                DrawFilledRect(x, y, 1, h - 1, br);
                DrawFilledRect(x + w - 1, y, 1, h, tl);
                DrawFilledRect(x, y + h - 1, w, 1, tl);
            }
            else
            {
                DrawFilledRect(x + w - 1, y, 1, h, br);
                DrawFilledRect(x, y + h - 1, w, 1, br);
                DrawFilledRect(x, y, w - 1, 1, tl);
                DrawFilledRect(x, y, 1, h - 1, tl);
            }
        }
        public static void DrawRect3D(Rectangle rect, Color tl, Color br, bool invert) { DrawRect3D(rect.X, rect.Y, rect.Width, rect.Height, tl, br, invert); }
        public static void DrawRect3D(Point pos, Point size, Color tl, Color br, bool invert) { DrawRect3D(pos.X, pos.Y, size.X, size.Y, tl, br, invert); }

        // draw character with transparent background
        public static unsafe void DrawChar(int x, int y, char c, Color color, Font font)
        {
            if (c == (char)0x20) { return; }
            int p = font.Height * (byte)c;
            for (int cy = 0; cy < font.Height; cy++)
            {
                for (byte cx = 0; cx < font.Width; cx++)
                {
                    if (Font.ConvertByteToBitAddress(font.Data[p + cy], cx + 1))
                    { DrawPixel(x + (font.Width - cx), y + cy, color); }
                }
            }
        }
        public static void DrawChar(Point pos, char c, Color color, Font font) { DrawChar(pos.X, pos.Y, c, color, font); }

        // draw character with colored background
        public static unsafe void DrawChar(int x, int y, char c, Color fg, Color bg, Font font)
        {
            int p = font.Height * (byte)c;
            for (int cy = 0; cy < font.Height; cy++)
            {
                for (byte cx = 0; cx < font.Width; cx++)
                {
                    if (Font.ConvertByteToBitAddress(font.Data[p + cy], cx + 1)) { DrawPixel(x + (font.Width - cx), y + cy, fg); }
                    else { DrawPixel(x + (font.Width - cx), y + cy, bg); }
                }
            }
        }
        public static void DrawChar(Point pos, char c, Color fg, Color bg, Font font) { DrawChar(pos.X, pos.Y, c, fg, bg, font); }

        // draw string with transparent background
        public static void DrawString(int x, int y, string text, Color color, Font font)
        {
            int xx = x, yy = y;
            for (int i = 0; i < text.Length; i++)
            {
                // new line
                if (text[i] == '\n') { xx = x; yy += font.Height + font.SpacingY; }
                // character
                else { DrawChar(xx, yy, text[i], color, font); xx += font.Width + font.SpacingX; }
            }
        }
        public static void DrawString(Point pos, string text, Color color, Font font) { DrawString(pos.X, pos.Y, text, color, font); }

        // draw string with colored background
        public static void DrawString(int x, int y, string text, Color fg, Color bg, Font font)
        {
            int xx = x, yy = y;
            for (int i = 0; i < text.Length; i++)
            {
                // new line
                if (text[i] == '\n') { xx = x; yy += font.Height + font.SpacingY; }
                // character
                else { DrawChar(xx, yy, text[i], fg, bg, font); xx += font.Width + font.SpacingX; }
            }
        }
        public static void DrawString(Point pos, string text, Color fg, Color bg, Font font) { DrawString(pos.X, pos.Y, text, fg, bg, font); }

        // draw image with default size
        public static void DrawImage(int x, int y, Image image)
        {
            for (int yy = 0; yy < image.Height; yy++)
            {
                for (int xx = 0; xx < image.Width; xx++)
                {
                    DrawPixel(x + xx, y + yy, (Color)image.Data[xx + (yy * image.Width)]);
                }
            }
        }
        public static void DrawImage(Point pos, Image image) { DrawImage(pos.X, pos.Y, image); }

        // draw image with default size and transparency key
        public static void DrawImage(int x, int y, Color transKey, Image image)
        {
            for (int yy = 0; yy < image.Height; yy++)
            {
                for (int xx = 0; xx < image.Width; xx++)
                {
                    if (image.Data[xx + (yy * image.Width)] != (byte)transKey)
                    { DrawPixel(x + xx, y + yy, (Color)image.Data[xx + (yy * image.Width)]); }
                }
            }
        }
        public static void DrawImage(Point pos, Color transKey, Image image) { DrawImage(pos.X, pos.Y, transKey, image); }

        // draw image with default size as single color
        public static void DrawImage(int x, int y, Color color, Color transKey, Image image)
        {
            for (int yy = 0; yy < image.Height; yy++)
            {
                for (int xx = 0; xx < image.Width; xx++)
                {
                    if (image.Data[xx + (yy * image.Width)] != (byte)transKey)
                    { DrawPixel(x + xx, y + yy, color); }
                }
            }
        }
        public static void DrawImage(Point pos, Color color, Color transKey, Image image) { DrawImage(pos.X, pos.Y, color, transKey, image); }
    }
}
