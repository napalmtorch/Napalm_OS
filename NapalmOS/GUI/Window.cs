using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Graphics;
using NapalmOS.Hardware;

namespace NapalmOS.GUI
{
    // structure for storing window flags
    public class WindowFlags
    {
        // flags
        public WindowStyle Style;
        public WindowState State;
        public bool Moving, Resizing, Running, Active;

        // constructor
        public WindowFlags(WindowStyle style = WindowStyle.Fixed, WindowState state = WindowState.Normal)
        {
            // set flags
            this.Style = style;
            this.State = state;
            this.Moving = false;
            this.Resizing = false;
            this.Running = false;
            this.Active = false;
        }
    }

    // window base class
    public abstract class Window
    {
        // dimensions
        public int X { get { return Bounds.X; } }
        public int Y { get { return Bounds.Y; } }
        public int Z = 0;
        public int Width { get { return Bounds.Width; } }
        public int Height { get { return Bounds.Height; } }
        public Rectangle Bounds, StartBounds;
        public Rectangle ClientBounds;
        private int mxStart, myStart;

        // properties
        public string ID { get; protected set; }
        public string Title { get; set; }
        public int Index { get; set; }
        public WindowFlags Flags;
        public MouseEventFlags MouseFlags;
        public VisualStyle Style { get; private set; }
        public bool ClientBoundsVisible { get; set; }
        public Core.Task Task { get; private set; }

        // scrolling
        public int ScrollX { get; protected set; }
        public int ScrollY { get; protected set; }

        // title bar
        public TitleBar TitleBar { get; private set; }
        private bool moveClick = false;

        // controls
        public List<Control> Controls { get; private set; } = new List<Control>();

        // constructor
        public Window(int x, int y, int w, int h, string title, string id)
        {
            // set dimensions
            SetBounds(x, y, w, h);

            // set properties
            this.ID = id;
            this.Title = title;
            this.Flags = new WindowFlags(WindowStyle.Fixed, WindowState.Normal);
            this.MouseFlags = new MouseEventFlags();
            this.Style = new VisualStyle();
            this.Style.Copy(VisualStyle.Window);
            this.TitleBar = new TitleBar(this);
            this.Flags.Active = false;
            this.ClientBoundsVisible = true;
            this.Task = new Core.Task(title, id);
            this.ScrollX = 0;
            this.ScrollY = 0;

            // set start bounds
            StartBounds.X = X;
            StartBounds.Y = Y;
            StartBounds.Width = Width;
            StartBounds.Height = Height;
        }

        // start
        public virtual void Start(string[] args)
        {
            Flags.Moving = false;
            Flags.Resizing = false;
            Flags.Running = true;
            Bounds.Set(StartBounds.X, StartBounds.Y, StartBounds.Width, StartBounds.Height);
            this.ScrollX = 0;
            this.ScrollY = 0;
            UpdateClientBounds();
        }

        // close
        public virtual void Close()
        {
            Flags.Running = false;
        }

        // update
        public virtual void Update()
        {
            // apply style if not already
            if (Style.Name == "Unassigned") { Style.Copy(VisualStyle.Window); }

            if (Flags.State != WindowState.Minimized)
            {
                if (Flags.State != WindowState.Fullscreen)
                {
                    // update client bounds
                    UpdateClientBounds();

                    // update title bar
                    TitleBar.Update(this);
                }

                if (Flags.Active)
                {
                    // handle moving
                    if (Flags.State == WindowState.Normal) { HandleMoving(); }

                    // update controls
                    if (Controls.Count > 0) { UpdateControls(); }
                }

                // activate
                if (WindowManager.Fullscreen == false && WindowManager.ActiveIndex == Index) { Flags.Active = true; }
            }
            else { Flags.Active = false; }

            // deactivate
            if (WindowManager.Fullscreen == false && WindowManager.ActiveIndex != Index) { Flags.Active = false; } 
        }

        // update controls
        public void UpdateControls()
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                // force deactivate if disabled
                if (Controls[i].Enabled == false) { Controls[i].Active = false; }

                // update control
                if (Flags.Active && Controls.Count > 0 && StartMenu.HoverCount == 0) { Controls[i].Update(this); }
            }
        }

        // update client bounds
        public void UpdateClientBounds()
        {
            if (Flags.State != WindowState.Fullscreen)
            { ClientBounds.Set(X + 1, Y + TitleBar.Height + 2, Width - 2, Height - (TitleBar.Height + 3)); }
            else { ClientBounds.Set(0, 0, VGADriver.Mode.Width, VGADriver.Mode.Height); }
        }

        // draw
        public virtual void Draw()
        {
            if (Flags.State != WindowState.Fullscreen)
            {
                // draw only border if moving
                if (Flags.Moving)
                {
                    Renderer.DrawRect(Bounds, 1, Style.Colors[3]);
                }
                // draw normally
                else
                {
                    if (Style.BorderStyle == BorderStyle.FixedSingle) { Renderer.DrawRect(Bounds, 1, Style.Colors[3]); }
                    else if (Style.BorderStyle == BorderStyle.Fixed3D) { Renderer.DrawRect3D(Bounds, Style.Colors[2], Style.Colors[3], false); }

                    // draw cross line
                    if (Style.BorderStyle != BorderStyle.None) { Renderer.DrawFilledRect(X + 1, Y + TitleBar.Height + 1, Width - 2, 1, Style.Colors[3]); }

                    // draw title bar
                    TitleBar.Draw(this);

                    // draw client bounds
                    if (ClientBoundsVisible) { Renderer.DrawFilledRect(ClientBounds, Style.Colors[0]); }

                    // debug text
                    //Renderer.DrawString(X, Y - 4, "ID=" + Index.ToString() + " ACTIVE=" + Flags.Active.ToString(), Color.White, Font.Font3x5);

                    // draw controls
                    if (Controls.Count > 0) { DrawControls(); }
                }
            }
            else { Shell.BackColor = Color.Red; }
        }

        // drwa controls
        public void DrawControls()
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].Visible) { Controls[i].Draw(this); }
            }
        }

        // handle movement
        private void HandleMoving()
        {
            if (MathUtil.RectangleContains(TitleBar.Bounds, MousePS2.X, MousePS2.Y) && !TitleBar.BtnClose.MouseFlags.Hover && !TitleBar.BtnMin.MouseFlags.Hover && WindowManager.ActiveIndex == Index)
            {
                if (MousePS2.State == Cosmos.System.MouseState.Left)
                {
                    if (!moveClick)
                    {
                        mxStart = MousePS2.X - X;
                        myStart = MousePS2.Y - Y;
                        moveClick = true;
                    }
                    Flags.Moving = true;
                }
            }

            // attempt to move window
            int newX = X, newY = Y;
            if (Flags.Moving)
            {
                newX = MousePS2.X - mxStart;
                newY = MousePS2.Y - myStart;
                SetPosition(newX, newY);
            }

            // window position limit
            if (X >= VGADriver.Mode.Width - 2) { Bounds.X = VGADriver.Mode.Width - 2; }
            if (Y >= VGADriver.Mode.Height - 2) { Bounds.Y = VGADriver.Mode.Height - 2; }

            // mouse release
            if (MousePS2.State == Cosmos.System.MouseState.None) { Flags.Moving = false; moveClick = false; }
        }

        // control management
        public void AddControl(Control control) { Controls.Add(control); }
        public void RemoveControl(Control control) { Controls.Remove(control); }
        public void RemoveControlAt(int i) { Controls.RemoveAt(i); }
        public void ClearControls() { Controls.Clear(); }
        public Control GetControl(int i) { if (i < Controls.Count) { return Controls[i]; } else { return null; } }

        // setters
        public void SetPosition(int x, int y) { this.Bounds.X = x; this.Bounds.Y = y; }
        public void SetSize(int w, int h) { this.Bounds.Width = w; this.Bounds.Height = h; }
        public void SetBounds(int x, int y, int w, int h) { this.Bounds.Set(x, y, w, h); }
        public void SetClientBounds(int x, int y, int w, int h) { this.ClientBounds.Set(x, y, w, h); }
        public void SetTitle(string text) { this.Title = text; }
    }
}
