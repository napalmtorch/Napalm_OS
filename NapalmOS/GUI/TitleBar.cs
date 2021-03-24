using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Graphics;
using NapalmOS.Hardware;

namespace NapalmOS.GUI
{
    public class TitleBar
    {
        // dimensions
        public const int Height = 9;
        public Rectangle Bounds;

        // buttons
        public Button BtnClose, BtnMin;

        // constructor
        public TitleBar(Window window)
        {
            // set dimensions
            this.SetBounds(window.X + 1, window.Y + 1, window.Width - 2, Height);

            // close button
            BtnClose = new Button(0, 0, "");
            BtnClose.SetSize(7, 7);

            // minimize button
            BtnMin = new Button(0, 0, "");
            BtnMin.SetSize(7, 7);
        }

        // update
        public void Update(Window window)
        {
            // update bounds
            this.SetBounds(window.X + 1, window.Y + 1, window.Width - 2, Height);

            // button positions
            BtnClose.SetPosition(window.X + window.Width - 9, window.Y + 2);
            BtnMin.SetPosition(BtnClose.X - 8, window.Y + 2);
            // close button
            if (window.Flags.State != WindowState.Minimized) { BtnClose.Update(null); }
            if (window.Flags.State == WindowState.Normal) { BtnClose.Update(null); BtnMin.Update(null); }
            BtnMin.Update(null);

            if (window.Flags.Active && WindowManager.MovingCount == 0)
            {
                // close clicked
                if (BtnClose.MouseFlags.Down && !BtnClose.MouseFlags.Clicked)
                {
                    WindowManager.Close(window.ID);
                    BtnClose.MouseFlags.Clicked = true;
                }

                // minimize clicked
                if (BtnMin.MouseFlags.Down && !BtnMin.MouseFlags.Clicked)
                {
                    window.Flags.State = WindowState.Minimized;
                    BtnMin.MouseFlags.Clicked = true;
                }

                // mouse release
                if (MousePS2.State == Cosmos.System.MouseState.None)
                { BtnClose.MouseFlags.Clicked = false; BtnMin.MouseFlags.Clicked = false; }
            }
        }

        // draw
        public void Draw(Window window)
        {
            // background
            Renderer.DrawFilledRect(Bounds, window.Style.Colors[4]);

            // title
            Renderer.DrawString(Bounds.X + 2, Bounds.Y + 2, window.Title, window.Style.Colors[5], Font.Font3x5);

            // draw buttons
            BtnClose.Draw(null);
            BtnMin.Draw(null);

            // draw close icon
            CheckBox.DrawCheckmark(BtnClose.X + 2, BtnClose.Y + 2, BtnClose.Style.Colors[1]);

            // draw min icon
            Renderer.DrawChar(BtnMin.X + 1, BtnMin.Y, '_', BtnMin.Style.Colors[1], Font.Font3x5);
        }

        // set bounds
        public void SetBounds(int x, int y, int w, int h) { this.Bounds.Set(x, y, w, h); }
    }
}
