using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.Graphics;
using NapalmOS.GUI;
using NapalmOS.Hardware;

namespace NapalmOS
{
    public static class Taskbar
    {
        // properties
        public static int Height = 11;
        public static bool Visible = true;

        public static Button BtnStart;

        // colors
        public static Color BackColor = Color.Silver4;

        // initialize
        public static void Initialize()
        {
            BtnStart = new Button(0, 0, "START");
        }

        // update
        public static void Update()
        {
            if (Visible)
            {
                BtnStart.SetPosition(2, VGADriver.Mode.Height - (Height - 1));
                BtnStart.Bounds.Width = 29;
                BtnStart.Update(null);

                // start button clicked
                if (BtnStart.MouseFlags.Down && !BtnStart.MouseFlags.Clicked) 
                { StartMenu.Visible = !StartMenu.Visible; BtnStart.MouseFlags.Clicked = true; }
            }
        }

        // draw
        public static void Draw()
        {
            // draw if visible
            if (Visible)
            {
                // draw background
                Renderer.DrawFilledRect(0, VGADriver.Mode.Height - Height, VGADriver.Mode.Width, Height, BackColor);

                // draw border
                Renderer.DrawFilledRect(0, VGADriver.Mode.Height - Height - 1, VGADriver.Mode.Width, 1, Color.White);
                Renderer.DrawFilledRect(0, VGADriver.Mode.Height - Height, 1, Height, Color.White);

                // draw time
                string time = Clock.GetTime(false, false);
                int w = Font.Font3x5.GetStringWidth(time);
                Renderer.DrawString(VGADriver.Mode.Width - w - 5, VGADriver.Mode.Height - 8, time, Color.Black, Font.Font3x5);

                BtnStart.Draw(null);
            }
        }
    }
}
