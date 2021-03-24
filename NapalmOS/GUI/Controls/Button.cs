using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Graphics;

namespace NapalmOS.GUI
{
    public class Button : Control
    {
        int txtW;

        // constructor
        public Button(int x, int y, string text) : base(x, y, 48, 9, ControlType.Button)
        {
            // set properties
            this.Text = text;
            this.Style.Copy(VisualStyle.Button);
        }

        // update
        public override void Update(Window window)
        {
            // update base
            base.Update(window);

            // apply style if not already
            if (Style.Name == "Unassigned") { Style.Copy(VisualStyle.Button); }

            // check default event flags
            if (Enabled) { CheckDefaultEvents(window); }
        }

        // draw
        public override void Draw(Window window)
        {
            // draw base
            base.Draw(window);

            // get font width
            txtW = Font.Font3x5.GetStringWidth(Text);

            // draw background
            Color bg = Style.Colors[0];
            Color fg = Style.Colors[1];
            if (!Enabled) { bg = Style.Colors[6]; fg = Style.Colors[7]; }
            if (window != null)
            {
                if (window.Style.Colors[0] != bg)
                { WindowRenderer.DrawFilledRect(X + 1, Y + 1, Width - 2, Height - 2, bg, window); }
            }
            else { WindowRenderer.DrawFilledRect(X + 1, Y + 1, Width - 2, Height - 2, bg, window); }

            // draw border
            WindowRenderer.DrawRect3D(X, Y, Width, Height, Style.Colors[2], Style.Colors[3], MouseFlags.Down, window);

            // draw text
            WindowRenderer.DrawString(X + (Width / 2) - (txtW / 2), Y + (Height / 2) - 2, Text, fg, Font.Font3x5, window);
        }
    }
}
