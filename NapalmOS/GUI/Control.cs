using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Graphics;
using NapalmOS.Hardware;

namespace NapalmOS.GUI
{
    // control types
    public enum ControlType
    {
        Button = 1,
        TextBox,
        CheckBox,
        ProgressBar,
        NumberPicker,
        ListBox,
    }

    // control base class
    public abstract class Control
    {
        // dimensions
        public int X { get { return Bounds.X; } }
        public int Y { get { return Bounds.Y; } }
        public int ScreenX { get; protected set; }
        public int ScreenY { get; protected set; }
        public int Width { get { return Bounds.Width; } }
        public int Height { get { return Bounds.Height; } }
        public Rectangle Bounds;

        // properties
        public ControlType Type { get; protected set; }
        public string Text;
        public bool Visible, Enabled, Active;
        public MouseEventFlags MouseFlags;
        public VisualStyle Style { get; private set; }

        // constructor
        public Control(int x, int y, int w, int h, ControlType type)
        {
            // set dimensions
            this.SetBounds(x, y, w, h);

            // set properties
            this.Type = type;
            this.Text = "CONTROL";
            this.Visible = true;
            this.Enabled = true;
            this.Active = false;
            this.MouseFlags = new MouseEventFlags();
            this.Style = new VisualStyle();
            this.ResetEventFlags();
        }

        // update
        public virtual void Update(Window window)
        {
            // get real position
            this.ScreenX = X + window.ClientBounds.X;
            this.ScreenY = Y + window.ClientBounds.Y;
        }

        // draw
        public virtual void Draw(Window window)
        {

        }

        // check for mouse events
        public void CheckDefaultEvents(Window window)
        {
            int xx = X, yy = Y;
            if (window != null) { xx = -window.ScrollX + ScreenX; yy = -window.ScrollY + ScreenY; }

            // if mouse is hovering over control
            if (MathUtil.RectangleContains(xx, yy, Width, Height, MousePS2.X, MousePS2.Y))
            {
                MouseFlags.Hover = true;
                if (MousePS2.State == Cosmos.System.MouseState.Left) { MouseFlags.Down = true; MouseFlags.Released = false; }
                if (MousePS2.State == Cosmos.System.MouseState.None) { MouseFlags.Down = false; MouseFlags.Clicked = false; MouseFlags.Released = true; }
            }
            else { MouseFlags.Down = false; MouseFlags.Clicked = false; MouseFlags.Hover = false; MouseFlags.Released = true; }

            // activate when clicked
            if (MouseFlags.Hover && MousePS2.State == Cosmos.System.MouseState.Left) { Active = true; }

            // deactivate when clicked away
            if (MousePS2.State == Cosmos.System.MouseState.Left && !MouseFlags.Hover) { Active = false; }
        }

        // reset event flags
        public void ResetEventFlags()
        {
            MouseFlags.Down = false;
            MouseFlags.Hover = false;
            MouseFlags.Clicked = false;
        }

        // setters
        public void SetPosition(int x, int y) { this.Bounds.X = x; this.Bounds.Y = y; }
        public void SetSize(int w, int h) { this.Bounds.Width = w; this.Bounds.Height = h; }
        public void SetBounds(int x, int y, int w, int h) { this.Bounds.Set(x, y, w, h); }
    }
}
