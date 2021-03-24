using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.GUI;
using NapalmOS.Hardware;

namespace NapalmOS.Graphics
{
    public static class WindowRenderer
    {
        // draw pixel to window bounds
        public static bool DrawPixel(int x, int y, Color color, Window window)
        {
            if (window != null)
            {
                // check if pixel is within client bounds
                if (!MathUtil.RectangleContains(window.ClientBounds, window.ClientBounds.X + x, window.ClientBounds.Y + y)) { return false; }
                // draw pixel
                VGADriver.DrawPixel(window.ClientBounds.X + x, window.ClientBounds.Y + y, color); return true;
            }
            else
            {
                // check if pixel is within client bounds
                if (!MathUtil.RectangleContains(0, 0, VGADriver.Mode.Width, VGADriver.Mode.Height, x, y)) { return false; }
                // draw pixel
                VGADriver.DrawPixel(x, y, color); return true;
            }
        }

        // draw filled rectangle
        public static void DrawFilledRect(int x, int y, int w, int h, Color color, Window window)
        {
            for (int i = 0; i < w * h; i++) { DrawPixel(x + (i % w), y + (i / w), color, window); }
        }
        public static void DrawFilledRect(Rectangle rect, Color color, Window window) { DrawFilledRect(rect.X, rect.Y, rect.Width, rect.Height, color, window); }
        public static void DrawFilledRect(Point pos, Point size, Color color, Window window) { DrawFilledRect(pos.X, pos.Y, size.X, size.Y, color, window); }

        // draw rectangle outline
        public static void DrawRect(int x, int y, int w, int h, int thick, Color color, Window window)
        {
            DrawFilledRect(x, y, w, thick, color, window);
            DrawFilledRect(x, y, thick, h, color, window);
            DrawFilledRect(x + w - thick, y, thick, h, color, window);
            DrawFilledRect(x, y + h - thick, w, thick, color, window);
        }
        public static void DrawRect(Rectangle rect, int thick, Color color, Window window) { DrawRect(rect.X, rect.Y, rect.Width, rect.Height, thick, color, window); }
        public static void DrawRect(Point pos, Point size, int thick, Color color, Window window) { DrawRect(pos.X, pos.Y, size.X, size.Y, thick, color, window); }

        // draw 3d bordered rectangle
        public static void DrawRect3D(int x, int y, int w, int h, Color tl, Color br, bool invert, Window window)
        {
            if (invert)
            {
                DrawFilledRect(x, y, w - 1, 1, br, window);
                DrawFilledRect(x, y, 1, h - 1, br, window);
                DrawFilledRect(x + w - 1, y, 1, h, tl, window);
                DrawFilledRect(x, y + h - 1, w, 1, tl, window);
            }
            else
            {
                DrawFilledRect(x + w - 1, y, 1, h, br, window);
                DrawFilledRect(x, y + h - 1, w, 1, br, window);
                DrawFilledRect(x, y, w - 1, 1, tl, window);
                DrawFilledRect(x, y, 1, h - 1, tl, window);
            }
        }
        public static void DrawRect3D(Rectangle rect, Color tl, Color br, bool invert, Window window) { DrawRect3D(rect.X, rect.Y, rect.Width, rect.Height, tl, br, invert, window); }
        public static void DrawRect3D(Point pos, Point size, Color tl, Color br, bool invert, Window window) { DrawRect3D(pos.X, pos.Y, size.X, size.Y, tl, br, invert, window); }

        // draw character with transparent background
        public static unsafe void DrawChar(int x, int y, char c, Color color, Font font, Window window)
        {
            if (c == (char)0x20) { return; }
            int p = font.Height * (byte)c;
            for (int cy = 0; cy < font.Height; cy++)
            {
                for (byte cx = 0; cx < font.Width; cx++)
                {
                    if (Font.ConvertByteToBitAddress(font.Data[p + cy], cx + 1))
                    { DrawPixel(x + (font.Width - cx), y + cy, color, window); }
                }
            }
        }
        public static void DrawChar(Point pos, char c, Color color, Font font, Window window) { DrawChar(pos.X, pos.Y, c, color, font, window); }

        // draw character with colored background
        public static unsafe void DrawChar(int x, int y, char c, Color fg, Color bg, Font font, Window window)
        {
            int p = font.Height * (byte)c;
            for (int cy = 0; cy < font.Height; cy++)
            {
                for (byte cx = 0; cx < font.Width; cx++)
                {
                    if (Font.ConvertByteToBitAddress(font.Data[p + cy], cx + 1)) { DrawPixel(x + (font.Width - cx), y + cy, fg, window); }
                    else { DrawPixel(x + (font.Width - cx), y + cy, bg, window); }
                }
            }
        }
        public static void DrawChar(Point pos, char c, Color fg, Color bg, Font font, Window window) { DrawChar(pos.X, pos.Y, c, fg, bg, font, window); }

        // draw string with transparent background
        public static void DrawString(int x, int y, string text, Color color, Font font, Window window)
        {
            int xx = x, yy = y;
            for (int i = 0; i < text.Length; i++)
            {
                // new line
                if (text[i] == '\n') { xx = x; yy += font.Height + font.SpacingY; }
                // character
                else { DrawChar(xx, yy, text[i], color, font, window); xx += font.Width + font.SpacingX; }
            }
        }
        public static void DrawString(Point pos, string text, Color color, Font font, Window window) { DrawString(pos.X, pos.Y, text, color, font, window); }

        // draw string with colored background
        public static void DrawString(int x, int y, string text, Color fg, Color bg, Font font, Window window)
        {
            int xx = x, yy = y;
            for (int i = 0; i < text.Length; i++)
            {
                // new line
                if (text[i] == '\n') { xx = x; yy += font.Height + font.SpacingY; }
                // character
                else { DrawChar(xx, yy, text[i], fg, bg, font, window); xx += font.Width + font.SpacingX; }
            }
        }
        public static void DrawString(Point pos, string text, Color fg, Color bg, Font font, Window window) { DrawString(pos.X, pos.Y, text, fg, bg, font, window); }

        // draw custom image format
        public static void DrawImage(int x, int y, Image image, Window window)
        {
            for (int yy = 0; yy < image.Height; yy++)
            {
                for (int xx = 0; xx < image.Width; xx++)
                {
                    DrawPixel(x + xx, y + yy, (Color)image.Data[xx + (yy * image.Width)], window);
                }
            }
        }
        public static void DrawImage(Point pos, Image image, Window window) { DrawImage(pos.X, pos.Y, image, window); }

        // draw custom image format with transparency key
        public static void DrawImage(int x, int y, Color transKey, Image image, Window window)
        {
            for (int yy = 0; yy < image.Height; yy++)
            {
                for (int xx = 0; xx < image.Width; xx++)
                {
                    if (image.Data[xx + (yy * image.Width)] != (byte)transKey)
                    { DrawPixel(x + xx, y + yy, (Color)image.Data[xx + (yy * image.Width)], window); }
                }
            }
        }
        public static void DrawImage(Point pos, Color transKey, Image image, Window window) { DrawImage(pos.X, pos.Y, transKey, image, window); }

        // draw custom image format as single color
        public static void DrawImage(int x, int y, Color color, Color transKey, Image image, Window window)
        {
            for (int yy = 0; yy < image.Height; yy++)
            {
                for (int xx = 0; xx < image.Width; xx++)
                {
                    if (image.Data[xx + (yy * image.Width)] != (byte)transKey)
                    { DrawPixel(x + xx, y + yy, color, window); }
                }
            }
        }
        public static void DrawImage(Point pos, Color color, Color transKey, Image image, Window window) { DrawImage(pos.X, pos.Y, color, transKey, image, window); }
    }
}
