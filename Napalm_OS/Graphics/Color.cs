using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using COL = System.Drawing.Color;

namespace Napalm_OS.Graphics
{
    public class Color
    {
        // properties
        public byte R, G, B;
        public uint PackedValue;

        // constructor
        public Color(byte r, byte g, byte b) { this.R = r; this.G = g; this.B = b; this.PackedValue = (uint)COL.FromArgb(r, g, b).ToArgb(); }
        public Color(uint packed) 
        {
            this.PackedValue = packed;
            COL col = COL.FromArgb((int)packed);
            this.R = col.R;
            this.G = col.G;
            this.B = col.B;
        }

        // convert colors to packed value
        public uint FromRGB(byte r, byte g, byte b) { return (uint)COL.FromArgb(r, g, b).ToArgb(); }
        public Color ToRGB(uint packed) { return new Color(COL.FromArgb((int)packed).R, COL.FromArgb((int)packed).G, COL.FromArgb((int)packed).B); }

        // colors
        public static Color Black { get; private set; } = new Color(0, 0, 0);
        public static Color White { get; private set; } = new Color(255, 255, 255);
        public static Color Red { get; private set; } = new Color(255, 0, 0);
        public static Color Blue { get; private set; } = new Color(0, 0, 255);
        public static Color Green { get; private set; } = new Color(0, 255, 0);
        public static Color Yellow { get; private set; } = new Color(255, 255, 0);
        public static Color Magenta { get; private set; } = new Color(255, 0, 255);
    }
}
