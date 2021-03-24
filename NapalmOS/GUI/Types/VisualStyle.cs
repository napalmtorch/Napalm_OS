using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Graphics;

namespace NapalmOS.GUI
{
    // color indices:
    // 0 = background
    // 1 = text
    // 2 = border top & left
    // 3 = border bottom & right / single border
    // 4 = hover / titlebar background
    // 5 = down  / titlebar text
    // 6 = disabled
    // 7 = disabled text

    // visual style base class
    public class VisualStyle
    {
        // colors
        public Color[] Colors { get; private set; }

        // flags
        public BorderStyle BorderStyle;
        
        // properties
        public string Name { get; private set; }
        public int ID { get; private set; }

        // constructor - default
        public VisualStyle()
        {
            // colors
            this.Colors = new Color[8];
            this.Colors[0] = Color.Black;
            this.Colors[1] = Color.Black;
            this.Colors[2] = Color.Black;
            this.Colors[3] = Color.Black;
            this.Colors[4] = Color.Black;
            this.Colors[5] = Color.Black;
            this.Colors[6] = Color.Black;
            this.Colors[7] = Color.Black;

            // flags
            this.BorderStyle = BorderStyle.None;

            // properties
            this.Name = "Unassigned";
            this.ID = 255;
        }

        // copy style
        public void Copy(VisualStyle style)
        {
            InitializeStyle(this, style.ID, style.Name, style.BorderStyle, style.Colors);
        }

        // default styles
        public static VisualStyle Window { get; private set; } = new VisualStyle();
        public static VisualStyle Button { get; private set; } = new VisualStyle();
        public static VisualStyle CheckBox { get; private set; } = new VisualStyle();
        public static VisualStyle TextBox { get; private set; } = new VisualStyle();

        // initialize default styles
        public static void InitializeDefaults()
        {
            InitializeStyle(Window,   0, "Default", BorderStyle.Fixed3D, new Color[] { Color.Silver5, Color.Black, Color.White, Color.Gray1, Color.Purple, Color.White, Color.Silver2, Color.Gray4 });
            InitializeStyle(Button,   1, "Default", BorderStyle.Fixed3D, new Color[] { Color.Silver5, Color.Black, Color.White, Color.Gray1, Color.Purple, Color.Purple4, Color.Silver2, Color.Gray4 });
            InitializeStyle(CheckBox, 2, "Default", BorderStyle.Fixed3D, new Color[] { Color.Silver5, Color.Black, Color.White, Color.Gray1, Color.Purple, Color.Purple4, Color.Silver2, Color.Gray4 });
            InitializeStyle(TextBox,  3, "Default", BorderStyle.Fixed3D, new Color[] { Color.White, Color.Black, Color.Black, Color.Gray, Color.Black, Color.Black, Color.Silver2, Color.Gray4 });
        }

        // initialize style
        public static void InitializeStyle(VisualStyle style, int id, string name, BorderStyle border, Color[] colors)
        {
            // colors
            for (int i = 0; i < 8; i++) { style.Colors[i] = colors[i]; }

            // flags
            style.BorderStyle = border;

            // properties
            style.Name = name;
            style.ID = id;
        }
    }
}
