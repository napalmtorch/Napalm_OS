using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Graphics;
using NapalmOS.Hardware;

namespace NapalmOS.GUI
{
    public class CheckBox : Control
    {
        // check
        public bool Checked = true;
        private bool click = false;

        // text width
        private int txtW;

        // constructor
        public CheckBox(int x, int y, string text) : base(x, y, 48, 9, ControlType.CheckBox)
        {
            // set properties
            this.Text = text;
            this.Style.Copy(VisualStyle.CheckBox);
        }

        // update
        public override void Update(Window window)
        {
            // update base
            base.Update(window);

            // force height
            Bounds.Height = 9;

            // apply style if not already
            if (Style.Name == "Unassigned") { Style.Copy(VisualStyle.CheckBox); }

            // get font width
            txtW = Font.Font3x5.GetStringWidth(Text);

            // check default event flags
            CheckDefaultEvents(window);

            // toggle checked state
            if (MouseFlags.Down && !click) { Checked = !Checked; click = true; }
            if (MousePS2.State == Cosmos.System.MouseState.None) { click = false; }
        }

        // draw
        public override void Draw(Window window)
        {
            // draw base
            base.Draw(window);

            // draw box inside
            if (window != null)
            {
                if (window.Style.Colors[0] != Style.Colors[0])
                { WindowRenderer.DrawFilledRect(X + 3, Y + 3, 3, 3, Style.Colors[0], window); }
            }
            else { WindowRenderer.DrawFilledRect(X + 3, Y + 3, 3, 3, Style.Colors[0], window); }

            // draw box
            WindowRenderer.DrawRect3D(X + 2, Y + 2, 5, 5, Style.Colors[2], Style.Colors[3], Checked, window);

            // draw check
            if (Checked) { DrawCheckmark(X + 3, Y + 3, Style.Colors[7], window); }

            // draw text
            WindowRenderer.DrawString(X + 7, Y + 2, Text, Style.Colors[1], Font.Font3x5, window);
        }

        // draw checkmark
        public static void DrawCheckmark(int x, int y, Color color, Window window)
        {
            WindowRenderer.DrawPixel(x, y, color, window);
            WindowRenderer.DrawPixel(x + 2, y, color, window);
            WindowRenderer.DrawPixel(x + 1, y + 1, color, window);
            WindowRenderer.DrawPixel(x, y + 2, color, window);
            WindowRenderer.DrawPixel(x + 2, y + 2, color, window);
        }

        public static void DrawCheckmark(int x, int y, Color color)
        {
            Renderer.DrawPixel(x, y, color);
            Renderer.DrawPixel(x + 2, y, color);
            Renderer.DrawPixel(x + 1, y + 1, color);
            Renderer.DrawPixel(x, y + 2, color);
            Renderer.DrawPixel(x + 2, y + 2, color);
        }
    }
}
